using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GlobalTimeline : TimelineBase<GlobalTimeline>
{
    private Dictionary<uint, CustomerData> _customers;

    public uint SecondsUntilClosure { get; private set; }

    public new void Start()
    {
        base.Start();
        
        SecondsUntilClosure = LevelManager.CurrentLevel.GetSeconds();
        _customers = LevelManager.CurrentLevel.Customers
            .ToDictionary(x => x.GetSeconds(), y => y);
        
        Tick += OnTimelineTick;
    }

    private void OnTimelineTick(object sender, EventArgs e)
    {
        SecondsUntilClosure--;
        HandleCustomerArrival();
        HandleStoreClosure();
        HandleTimerDisplay();
    }

    private void HandleTimerDisplay()
    {
        Sidebar.Instance.DaytimeDisplay.UpdateTime((int)SecondsUntilClosure);
    }

    private void HandleStoreClosure()
    {
        Sidebar.Instance.OpenStatus.UpdateTime((int)SecondsUntilClosure);
        if (SecondsUntilClosure != 0) return;
        StartCoroutine(nameof(CloseStore));
    }

    private IEnumerator CloseStore()
    {
        yield return CustomerHandler.Instance.WaitUntilCustomersLeave();
        MainMenu.Instance.CompleteDay();
        Active = false;
    }

    private void HandleCustomerArrival()
    {
        if (!_customers.ContainsKey((uint)Ticks + 1)) return;
        
        var customer = _customers
            .First(x => x.Key == (uint)Ticks + 1)
            .Value;

        CustomerHandler.Instance.SummonNewCustomer(customer);
    }
}
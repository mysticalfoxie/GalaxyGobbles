using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GlobalTimeline : TimelineBase<GlobalTimeline>
{
    private Dictionary<uint, CustomerData> _customers;

    public uint SecondsUntilClosure { get; private set; }

    public override void Awake()
    {
        base.Awake();

        var operation = WaitUntilLevelLoaded(() =>
        {
            SecondsUntilClosure = LevelManager.CurrentLevel.GetSeconds();
            _customers = LevelManager.CurrentLevel.Customers
                .ToDictionary(x => x.GetSeconds(), y => y);
            
            Tick += OnTimelineTick;
        });
        
        StartCoroutine(operation);
    }

    private static IEnumerator WaitUntilLevelLoaded(Action callback)
    {
        yield return new WaitUntil(() => !LevelManager.Instance.Loading);
        callback();
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
        BottomBar.Instance.DaytimeDisplay.UpdateTime((int)SecondsUntilClosure);
    }

    private void HandleStoreClosure()
    {
        BottomBar.Instance.OpenStatus.UpdateTime((int)SecondsUntilClosure);
        if (SecondsUntilClosure != 0) return;
        StartCoroutine(nameof(CloseStore));
    }

    private void HandleCustomerArrival()
    {
        if (!_customers.ContainsKey((uint)Ticks + 1)) return;
        
        var customer = _customers
            .First(x => x.Key == (uint)Ticks + 1)
            .Value;

        CustomerHandler.Instance.SummonNewCustomer(customer);
    }

    private IEnumerator CloseStore()
    {
        yield return CustomerHandler.Instance.WaitUntilCustomersLeave();
        MainMenu.Instance.CompleteDayMenu();
        Active = false;
    }
}
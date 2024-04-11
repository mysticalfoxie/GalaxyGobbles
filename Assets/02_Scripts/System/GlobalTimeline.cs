using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GlobalTimeline : TimelineBase
{
    private Dictionary<uint, CustomerData> _customers;

    public static GlobalTimeline Instance { get; private set; }
    public uint SecondsUntilClosure { get; private set; }
    public int Seconds => Ticks;

    public void Awake()
    {
        if (Instance is not null)
        {
            Destroy(this);
            return;
        }
        
        Instance = this;
    }

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
        HandleCustomerArrival();
        HandleStoreClosure();
    }

    private void HandleStoreClosure()
    {
        if ((uint)Ticks + 1 != SecondsUntilClosure) return;
        StartCoroutine(nameof(WaitUntilCustomersLeave));
    }

    private IEnumerator WaitUntilCustomersLeave()
    {
        yield return new WaitWhile(() => _customers.Count != 0 );
        Debug.Log	("Complete");
        MainMenu.Instance.CompleteDay();
    }

    private void HandleCustomerArrival()
    {
        if (!_customers.ContainsKey((uint)Ticks + 1)) return;
        
        var customer = _customers
            .First(x => x.Key == (uint)Ticks + 1)
            .Value;

        SummonNewCustomer(customer);
    }
    
    private static void SummonNewCustomer(CustomerData data)
    {
        var customerGameObject = Instantiate(LevelManager.Instance._customerPrefab);
        var customer = customerGameObject.GetComponent<Customer>();
        customer.Data = data;
    }
}
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaitAreaHandler : SingletonMonoBehaviour<WaitAreaHandler>
{
    private readonly List<Customer> _outsideQueue = new();
    private WaitArea[] _waitAreas;
    
    public override void Awake()
    {
        base.Awake();
        
        _waitAreas = GetComponentsInChildren<WaitArea>()
            .OrderBy(x => x.Order)
            .ToArray();
    }

    public void AddCustomer(Customer customer)
    {
        var slot = _waitAreas.FirstOrDefault(x => x.Customer is null);
        if (slot is null)
        {
            _outsideQueue.Add(customer);
            customer.gameObject.SetActive(false);
            Debug.LogWarning("[Game Design] There are not enough slots for the customer to arrive in the store.");
            Debug.LogWarning("[Game Design] Customer waits outside and will arrive as soon as there's an open slot.");
            return;
        }
        
        slot.SetCustomer(customer);
    }

    public void RemoveCustomer(Customer customer)
    {
        var slot = _waitAreas.First(x => x.Customer == customer);
        slot.RemoveCustomer();
        
        RestockSlots();
    }

    private void RestockSlots()
    {
        StartCoroutine(nameof(OnRestockCustomers));
    }

    private IEnumerator OnRestockCustomers()
    {
        yield return new WaitForSeconds(GameSettings.Data.RestockCustomerDelay);
        for (var i = 0; i < _waitAreas.Length; i++)
        {
            var customer = GetCustomerFromNextSlot(i);
            if (customer is null)
                _waitAreas[i].RemoveCustomer();
            else
                _waitAreas[i].SetCustomer(customer);
        }
    }

    private Customer GetCustomerFromNextSlot(int iterator)
    {
        if (_waitAreas.Length == iterator + 1)
        {
            var customer = _outsideQueue.FirstOrDefault();
            if (customer is not null)
                _outsideQueue.Remove(customer);
            
            return customer; // Nullable!!
        }

        var nextSlot = _waitAreas[iterator + 1];
        var customerO = nextSlot.Customer;
        nextSlot.RemoveCustomer();
        
        return customerO;
    }
}
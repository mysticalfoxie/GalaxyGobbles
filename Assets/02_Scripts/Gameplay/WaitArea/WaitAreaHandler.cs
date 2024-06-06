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
            .OrderByDescending(x => x.Order)
            .ToArray();
    }

    public void AddCustomer(Customer customer)
    {
        var slot = _waitAreas.FirstOrDefault(x => x.Customer is null);
        if (slot is null)
        {
            _outsideQueue.Add(customer);
            customer.Visible = false;
            customer.gameObject.SetActive(false);
            
            Debug.Log("[Wait Area] A new customer waits outside and will arrive as soon as there's an open slot.");
            return;
        }
        
        slot.SetCustomer(customer);
        customer.Visible = true;
    }

    public void RemoveCustomer(Customer customer)
    {
        if (customer is null) return;
        
        var slot = _waitAreas.FirstOrDefault(x => x.Customer == customer);
        if (slot is null) return;
        slot.RemoveCustomer();
        
        RestockSlots();
    }

    private void RestockSlots()
    {
        StartCoroutine(nameof(OnRestockCustomers));
    }

    private IEnumerator OnRestockCustomers()
    {
        yield return new WaitForSeconds(GameSettings.Data.QueueRestockDelay);
        for (var i = 0; i < _waitAreas.Length; i++)
        {
            if (_waitAreas[i].Customer is not null) continue;
            
            var customer = GetCustomerFromNextSlot(i);
            if (customer is null) continue;
            
            _waitAreas[i].SetCustomer(customer);
            customer.Visible = true;
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
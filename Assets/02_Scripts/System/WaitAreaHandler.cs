using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaitAreaHandler : MonoBehaviour
{
    public static WaitAreaHandler Instance;
    private readonly List<Customer> _outsideQueue = new();
    private WaitArea[] _waitAreas;
    
    public void Awake()
    {
        if (Instance is not null)
        {
            Destroy(gameObject);
            return;
        }

        _waitAreas = GetComponentsInChildren<WaitArea>()
            .OrderBy(x => x.Order)
            .ToArray();
        
        Instance = this;
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
        
        slot.Customer = customer;
        customer.gameObject.transform.position = slot.gameObject.transform.position;
    }

    public void RemoveCustomer(Customer customer)
    {
        var slot = _waitAreas.First(x => x.Customer == customer);
        slot.Customer = null;
        
        RestockSlots();
    }

    private void RestockSlots()
    {
        for (var i = 0; i < _waitAreas.Length; i++)
        {
            _waitAreas[i].Customer ??= GetCustomerFromNextSlot(i);
            if (_waitAreas[i].Customer is null) continue;
            _waitAreas[i].Customer.gameObject.transform.position = _waitAreas[i].gameObject.transform.position;
            _waitAreas[i].Customer.gameObject.SetActive(true);
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
        nextSlot.Customer = null; // TODO: Might kill the reference... Testing
        
        return customerO;
    }
}
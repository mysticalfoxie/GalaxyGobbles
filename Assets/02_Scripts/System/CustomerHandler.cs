using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerHandler : SingletonMonoBehaviour<CustomerHandler>
{
    private readonly List<Customer> _customers = new();
    public IEnumerable<Customer> Customers => _customers;

    public void SummonNewCustomer(CustomerData data)
    {
        var customer = Customer.Create(data);
        _customers.Add(customer);
        WaitAreaHandler.Instance.AddCustomer(customer);
        customer.Leave += OnCustomerLeave;
    }

    private void OnCustomerLeave(object sender, EventArgs e)
    {
        if (sender is not Customer customer) return;
        customer.Leave -= OnCustomerLeave;
        _customers.Remove(customer);
    }

    public IEnumerator WaitUntilCustomersLeave()
    {
        yield return new WaitWhile(() => _customers.Count != 0);
    }
}
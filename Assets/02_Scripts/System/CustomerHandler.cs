using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerHandler : Singleton<CustomerHandler>
{
    private readonly List<Customer> _customers = new();
    public IEnumerable<Customer> Customers => _customers;

    public void SummonNewCustomer(CustomerData data)
    {
        var customer = Customer.Create(data);
        _customers.Add(customer);
        WaitAreaHandler.Instance.AddCustomer(customer);
        customer.Destroying += OnCustomerDestroyed;
    }

    private void OnCustomerDestroyed(object sender, EventArgs e)
    {
        if (sender is not Customer customer) return;
        customer.Destroying -= OnCustomerDestroyed;
        _customers.Remove(customer);
    }

    public IEnumerator WaitUntilCustomersLeave(Func<bool> cancellation)
    {
        yield return new WaitWhile(() => _customers.Count != 0 && !cancellation());
        yield return new CancellableWaitForSeconds(GameSettings.Data.ClosureDelay, cancellation);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerHandler : MonoBehaviour
{
    private readonly List<Customer> _customers = new();
    public IEnumerable<Customer> Customers => _customers;

    public static CustomerHandler Instance { get; private set; }

    public void Awake()
    {
        if (Instance is not null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }


    public void SummonNewCustomer(CustomerData data)
    {
        var customerGameObject = Instantiate(LevelManager.Instance._customerPrefab);
        var customer = customerGameObject.GetComponent<Customer>();
        customer.Data = data;
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
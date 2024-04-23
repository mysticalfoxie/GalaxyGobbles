using System;
using System.Linq;
using UnityEngine;

public class TableDistributor : MonoBehaviour
{
    private Table[] _tables;
    
    public void Awake()
    {
        _tables = GetComponentsInChildren<Table>();
        foreach (var table in _tables) 
            table.Click += (_, _) => OnTableClick(table);
    }

    private void OnTableClick(Table table)
    {
        if (table.SeatedCustomer is not null)
        {
            table.SeatedCustomer.TryReceiveMeal();
            table.SeatedCustomer.TryCheckout();
            return;
        }

        if (SelectionHandler.Instance.Selection is not Customer customer) return;
        if (customer.State != CustomerState.WaitingForSeat) return;

        customer.Leave += OnCustomerLeave;
        WaitAreaHandler.Instance.RemoveCustomer(customer);
        table.Seat(customer);
        customer.Deselect();
        customer.OnSeated();
    }

    private void OnCustomerLeave(object sender, EventArgs e)
    {
        if (sender is not Customer customer) return;
        var table = _tables.FirstOrDefault(x => x.SeatedCustomer == customer);
        if (table is null) return;
        customer.Leave -= OnCustomerLeave;
        table.ClearSeat();
    }
}
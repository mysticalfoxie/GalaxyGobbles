using System;
using System.Linq;
using UnityEngine;

public class TableDistributor : MonoBehaviour
{
    public void Awake()
    {
        foreach (var table in References.Instance.Tables) 
            table.Touch += (_, _) => OnTableClick(table);
    }

    private static void OnTableClick(Table table)
    {
        if (table.SeatedCustomer is not null)
        {
            if (SelectionSystem.Instance.Selection is not null)
                SelectionSystem.Instance.Selection.Deselect();
            
            table.SeatedCustomer.TryReceiveMeal();
            table.SeatedCustomer.TryCheckout();
            return;
        }

        if (SelectionSystem.Instance.Selection is not Customer customer) return;
        if (customer.State != CustomerState.WaitingForSeat) return;

        customer.Leave += OnCustomerLeave;
        WaitAreaHandler.Instance.RemoveCustomer(customer);
        table.Seat(customer);
        customer.Deselect();
        customer.OnSeated();
    }

    private static void OnCustomerLeave(object sender, EventArgs e)
    {
        if (sender is not Customer customer) return;
        var table = References.Instance.Tables.FirstOrDefault(x => x.SeatedCustomer == customer);
        if (table is null) return;
        customer.Leave -= OnCustomerLeave;
        table.ClearSeat();
    }
}
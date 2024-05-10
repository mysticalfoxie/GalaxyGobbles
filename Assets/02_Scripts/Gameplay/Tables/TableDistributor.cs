using System;
using System.Linq;
using UnityEngine;

public class TableDistributor : MonoBehaviour
{
    public void Start()
    {
        foreach (var table in References.Instance.Tables) 
            table.Touch += (_, _) => OnTableClick(table);
        foreach (var chair in References.Instance.Chairs)
            chair.Touch += (_, _) => OnChairClick(chair);
    }

    private static void OnChairClick(Chair chair) 
        => OnTableClick(chair.Table, chair);

    private static void OnTableClick(Table table, Chair chair = null)
    {
        if (TryHandleSeatedCustomer(table)) return;
        HandleTableAssignment(table, chair);
    }

    private static void HandleTableAssignment(Table table, Chair chair)
    {
        if (SelectionSystem.Instance.Selection is not Customer customer) return;
        if (customer.StateMachine.State != CustomerState.WaitingForSeat) return;

        customer.Leave += OnCustomerLeave;
        WaitAreaHandler.Instance.RemoveCustomer(customer);
        table.Seat(customer, chair);
        customer.Deselect();
        customer.OnSeated();
    }

    private static bool TryHandleSeatedCustomer(Table table)
    {
        if (table.Customer is null) return false;
        if (SelectionSystem.Instance.Selection is not null)
        {
            SelectionSystem.Instance.Deselect();
            return true;
        }
            
        table.Customer.TryReceiveMeal();
        table.Customer.TryCheckout();
        
        return true;
    }

    private static void OnCustomerLeave(object sender, EventArgs e)
    {
        if (sender is not Customer customer) return;
        var table = References.Instance.Tables.FirstOrDefault(x => x.Customer == customer);
        if (table is null) return;
        customer.Leave -= OnCustomerLeave;
        table.ClearSeat();
    }
}
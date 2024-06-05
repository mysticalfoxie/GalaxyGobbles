using System;

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tables : MonoBehaviour
{
    public List<(Table, Table)> TablePairs = new();
    
    public void Awake()
    {
        FindTablePairs();
    }

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
        if (table.RequiresCleaning) return;

        customer.Destroying += OnCustomerDestroyed;
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

    private void FindTablePairs()
    {
        var tables = References.Instance.Tables.ToArray();
        foreach (var table in tables)
        {
            if (table.NeighbourTable is null) throw new Exception($"The table with name '{table.name}' does not have a neighbour table defined!");
            if (table.NeighbourTable.NeighbourTable != table) continue;
            if (TablePairs.Any(x => x.Item1 == table || x.Item2 == table)) continue;
            TablePairs.Add((table, table.NeighbourTable));
        }
    }

    private static void OnCustomerDestroyed(object sender, EventArgs e)
    {
        if (sender is not Customer customer) return;
        var table = References.Instance.Tables.FirstOrDefault(x => x.Customer == customer);
        if (table is null) return;
        customer.Destroying -= OnCustomerDestroyed;
        
        if (customer.StateMachine.State == CustomerState.Dying)
            customer.Table.SetDirty();
        
        table.ClearSeat();
    }
}
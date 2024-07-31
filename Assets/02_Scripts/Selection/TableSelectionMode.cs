using System;
using System.Linq;
using UnityEngine;

public class TableSelectionHandler : ISelectionHandler
{
    public static TableSelectionHandler Instance { get; } = new();

    public void OnEnable()
    {
        var chairs = References.Instance.Chairs
            .Where(x => x && x.isActiveAndEnabled && x.Table)
            .Where(x => x.Table.Customer is null);

        foreach (var chair in chairs)
            chair.Animator.StartPulsating();
    }

    public event EventHandler<object> Result;
    public event EventHandler Cancel;

    public void OnGameObjectTouched(GameObject @object, TouchEvent eventArgs)
    {
        // ReSharper disable once ConvertIfStatementToSwitchStatement
        var touchable = @object.GetComponents<Touchable>().FirstOrDefault();
        if (!IsValidTouch(touchable)) return;
        
        eventArgs.CancelPropagation();

        if (GetTouchedTable(@object, out var table))
            Result?.Invoke(this, table);
        else
            Cancel?.Invoke(this, null);

        SelectionSystem.Instance.Deselect();
    }

    private bool IsValidTouch(Touchable touchable)
    {
        if (touchable && !touchable.CancelSelectionOnTouch) return true;
        
        Cancel?.Invoke(this, null);
        SelectionSystem.Instance.Deselect();
        
        return true;
    }

    private static bool GetTouchedTable(GameObject @object, out TableSelectEvent eventArgs)
    {
        var chairs = @object.GetComponents<Chair>();
        var tables = @object.GetComponents<Table>();

        if (chairs.Length > 0)
        {
            var chair = chairs.First();
            eventArgs = new TableSelectEvent
            {
                Table = chair.Table,
                Chair = chair
            };
            
            return true;
        }

        if (tables.Length > 0)
        {
            eventArgs = new TableSelectEvent { Table = tables.First() };
            return true;
        }

        eventArgs = null;
        return false;
    }

    public void OnSelectableTouched(Selectable selectable)
    {
        if (selectable.Selected)
        {
            selectable.Deselect();
            return;
        }

        SelectionSystem.Instance.Select(selectable);
    }

    public void OnDisable()
    {
        var chairs = References.Instance.Chairs
            .Where(x => x && x.isActiveAndEnabled && x.Table);
        
        foreach (var chair in chairs)
            chair.Animator.StopPulsating();
    }
}
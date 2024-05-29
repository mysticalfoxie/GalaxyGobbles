using System;
using System.Linq;
using UnityEngine;

public class TableSelectionHandler : ISelectionHandler
{
    public static TableSelectionHandler Instance { get; } = new();

    public void OnEnable()
    {
        Result?.Invoke(null, null);
        Cancel?.Invoke(null, null);
    }

    public event EventHandler<object> Result;
    public event EventHandler Cancel;

    public void OnGameObjectTouched(GameObject @object)
    {
        // ReSharper disable once ConvertIfStatementToSwitchStatement
        if (@object.GetComponents<TouchableMonoBehaviour>().FirstOrDefault() is { CancelSelectionOnTouch: false }) return;
        if (SelectionSystem.Instance.Selection is null) return;
        SelectionSystem.Instance.Deselect();
    }

    public void OnSelectableTouched(SelectableMonoBehaviour selectable)
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
        
    }
}
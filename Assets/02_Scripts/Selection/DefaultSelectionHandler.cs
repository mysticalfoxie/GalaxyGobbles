using System;
using System.Linq;
using UnityEngine;

public class DefaultSelectionHandler : ISelectionHandler
{
    public static DefaultSelectionHandler Instance { get; } = new();

    public void OnEnable()
    {
        // Just to avoid unused warnings. -> DefaultSelection does not require these events
        Result?.Invoke(null, null);
        Cancel?.Invoke(null, null);
    }

    public event EventHandler<object> Result;
    public event EventHandler Cancel;

    public void OnGameObjectTouched(GameObject @object, TouchEvent eventArgs)
    {
        // ReSharper disable once ConvertIfStatementToSwitchStatement
        if (@object.GetComponents<TouchableMonoBehaviour>().FirstOrDefault() is { CancelSelectionOnTouch: false }) return;
        if (SelectionSystem.Instance.Selection is null) return;
        SelectionSystem.Instance.Deselect();
    }

    public void OnSelectableTouched(SelectableMonoBehaviour selectable)
    {
        if (selectable.Selected) 
            selectable.Deselect();
        else
            SelectionSystem.Instance.Select(selectable);
    }

    public void OnDisable()
    {
        
    }
}
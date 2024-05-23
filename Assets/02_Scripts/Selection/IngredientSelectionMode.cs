using System.Linq;
using UnityEngine;

public class IngredientSelectionHandler : ISelectionHandler
{
    public static IngredientSelectionHandler Instance { get; } = new();

    public void OnEnable()
    {
        
    }
    
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
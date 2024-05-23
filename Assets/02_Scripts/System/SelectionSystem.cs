using System;
using System.Linq;
using UnityEngine;

public class SelectionSystem : SingletonMonoBehaviour<SelectionSystem>
{
    public SelectableMonoBehaviour Selection { get; private set; }

    private SelectionMode _selectionMode;

    public SelectionMode SelectionMode
    {
        get => _selectionMode;
        set => OnSelectionModeUpdate(value);
    } 
    
    public void Start() 
        => TouchHandler.Instance.Touch += OnGameObjectTouched;
    
    public void Register(TouchableMonoBehaviour touchable) 
        => touchable.Touch += OnSelectableMonoBehaviourTouched;

    private void OnGameObjectTouched(object sender, EventArgs e)
    {
        if (sender is not GameObject @object) return;
        if (@object.GetComponents<SelectableMonoBehaviour>().Any()) return;
        var handler = GetHandlerForSelectionMode();
        handler.OnGameObjectTouched(@object);
    }

    private void OnSelectableMonoBehaviourTouched(object sender, EventArgs e)
    {
        if (sender is not SelectableMonoBehaviour selectable) return;
        var handler = GetHandlerForSelectionMode();
        handler.OnSelectableTouched(selectable);
    }

    public void Deselect()
    {
        Selection.Deselect();
        Selection = null;
    }

    public void Select(SelectableMonoBehaviour selectable) {
        if (!selectable.IsSelectable()) return;
        if (Selection is not null)
            Selection.Deselect();

        selectable.Select();
        Selection = selectable;
    }

    private void OnSelectionModeUpdate(SelectionMode value)
    {
        GetHandlerForSelectionMode(_selectionMode).OnDisable();
        _selectionMode = value;
        GetHandlerForSelectionMode(_selectionMode).OnEnable();
    }
    
    private ISelectionHandler GetHandlerForSelectionMode(SelectionMode? mode = null)
        => (mode ?? SelectionMode) switch
        {
            SelectionMode.Ingredients => IngredientSelectionHandler.Instance,
            SelectionMode.Tables => TableSelectionHandler.Instance, 
            _ => DefaultSelectionHandler.Instance
        };
}
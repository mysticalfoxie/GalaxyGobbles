using System;
using System.Collections;
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

    public IEnumerator WaitForIngredientSelection(Action<ItemData> callback)
    {
        yield return WaitForObjectSelection(callback, SelectionMode.Ingredients);
    }

    public IEnumerator WaitForTableSelection(Action<Table> callback)
    {
        yield return WaitForObjectSelection(callback, SelectionMode.Tables);
    }

    private void OnGameObjectTouched(object sender, TouchEvent eventArgs)
    {
        if (sender is not GameObject @object) return;
        if (@object.GetComponents<SelectableMonoBehaviour>().Any()) return;
        var handler = GetHandlerForSelectionMode();
        handler.OnGameObjectTouched(@object, eventArgs);
    }

    private void OnSelectableMonoBehaviourTouched(object sender, EventArgs e)
    {
        if (sender is not SelectableMonoBehaviour selectable) return;
        var handler = GetHandlerForSelectionMode();
        handler.OnSelectableTouched(selectable);
    }

    private void OnSelectionModeUpdate(SelectionMode value)
    {
        GetHandlerForSelectionMode(_selectionMode).OnDisable();
        _selectionMode = value;
        GetHandlerForSelectionMode(_selectionMode).OnEnable();
    }

    private IEnumerator WaitForObjectSelection<T>(Action<T> callback, SelectionMode mode) where T : class
    {
        SelectionMode = mode;
        var handler = GetHandlerForSelectionMode();
        handler.Result += OnSelectionResult;
        handler.Cancel += OnSelectionCancelled;
        var waiting = true;

        yield return new WaitWhile(() => waiting);
        SelectionMode = SelectionMode.Default;
        yield break;

        void OnSelectionResult(object sender, object @object)
        {
            handler.Result -= OnSelectionResult;
            handler.Cancel -= OnSelectionCancelled;
            if (@object is T t) 
                callback(t);
            else
                callback(null);
        
            waiting = false;
        }

        void OnSelectionCancelled(object sender, EventArgs e)
        {
            handler.Result -= OnSelectionResult;
            handler.Cancel -= OnSelectionCancelled;
            callback(null);
            waiting = false;
        }
    } 
    
    private ISelectionHandler GetHandlerForSelectionMode(SelectionMode? mode = null)
        => (mode ?? SelectionMode) switch
        {
            SelectionMode.Ingredients => IngredientSelectionHandler.Instance,
            SelectionMode.Tables => TableSelectionHandler.Instance, 
            _ => DefaultSelectionHandler.Instance
        };
}
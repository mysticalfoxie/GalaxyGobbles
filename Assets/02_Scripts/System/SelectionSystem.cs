using System;
using System.Linq;
using UnityEngine;

public class SelectionSystem : SingletonMonoBehaviour<SelectionSystem>
{
    public SelectableMonoBehaviour Selection { get; private set; }

    public void Start()
    {
        TouchHandler.Instance.Touch += OnGameObjectTouched;
    }

    private void OnGameObjectTouched(object sender, EventArgs e)
    {
        // ReSharper disable once ConvertIfStatementToSwitchStatement
        if (sender is not GameObject @object) return;
        if (@object.GetComponents<SelectableMonoBehaviour>().Any()) return;
        if (@object.GetComponents<TouchableMonoBehaviour>().FirstOrDefault() is { CancelSelectionOnClick: false }) return;
        if (Selection is null) return;
        Selection.Deselect();
        Selection = null;
    }

    public void Register(TouchableMonoBehaviour touchable)
    {
        touchable.Click += (@object, _) =>
        {
            if (@object is not SelectableMonoBehaviour se)
                return;

            HandleSelection(se);
        };
    }

    private void HandleSelection(SelectableMonoBehaviour selectable)
    {
        if (selectable.Selected)
        {
            selectable.Deselect();
            return;
        }

        if (!selectable.IsSelectable()) return;
        if (Selection is not null)
            Selection.Deselect();

        selectable.Select();
        Selection = selectable;
    }
}
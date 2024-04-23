using System;
using System.Linq;
using UnityEngine;

public class SelectionHandler : SingletonMonoBehaviour<SelectionHandler>
{
    public SelectableMonoBehaviour Selection { get; private set; }

    public void Register(TouchableMonoBehaviour touchable)
    {
        touchable.Click += (@object, _) =>
        {
            if (@object is not SelectableMonoBehaviour se)
                return;

            HandleSelection(se);
        };
    }

    public void Update()
    {
        HandleTapping();
    }

    private void HandleTapping()
    {
        if (!TouchEventSystem.Instance.IsTapping) return;
        var tapped = TouchEventSystem.Instance.GetTappedGameObject();
        if (tapped is null) return;
        if (TryHandleSelection(tapped)) return;
        if (Selection is not null) Selection.Deselect();
        HandleTouches(tapped);
    }

    private void HandleTouches(GameObject tapped)
    {
        var touchables = tapped.GetComponents<TouchableMonoBehaviour>();
        if (touchables.Length == 0) return;
        var touchable = touchables.First();
        touchable.InvokeClick(this, EventArgs.Empty);
    }

    private bool TryHandleSelection(GameObject tapped)
    {
        var selectables = tapped.GetComponents<SelectableMonoBehaviour>();
        if (selectables.Length == 0) return false;
        var selectable = selectables.First();
        HandleSelection(selectable);
        return true;
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
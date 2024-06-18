using System;
using System.Linq;
using UnityEngine;

// ReSharper disable ConvertIfStatementToSwitchStatement

public class IngredientSelectionHandler : ISelectionHandler
{
    public static IngredientSelectionHandler Instance { get; } = new();

    public void OnEnable()
    {
        
    }

    public event EventHandler<object> Result;
    public event EventHandler Cancel;

    public void OnGameObjectTouched(GameObject @object, TouchEvent eventArgs)
    {
        var canCancelSelection = @object.GetComponents<Touchable>().FirstOrDefault() is not { CancelSelectionOnTouch: false };
        var itemProvider = @object.GetComponent<ItemProvider>() ?? GetItemProviderFromItemRenderer(@object);
        
        eventArgs.CancelPropagation();
        
        if (!canCancelSelection && itemProvider is null) return;
        if (canCancelSelection && itemProvider is null) {
            Cancel?.Invoke(this, null);
            return;
        }

        var itemProviderItem = GameSettings.GetItemMatch(itemProvider.Item);
        if (itemProviderItem.CanBecomePoison)
            Result?.Invoke(this, itemProviderItem);
        else if (canCancelSelection)
            Cancel?.Invoke(this, null);
    }

    private static ItemProvider GetItemProviderFromItemRenderer(GameObject gameObject)
    {
        var renderer = gameObject.GetComponent<ItemRenderer>();
        if (renderer is null) return null;
        if (renderer.Initiator is not ItemProvider provider) return null;
        return provider;
    }

    public void OnSelectableTouched(Selectable selectable)
    {
        if (selectable.GetComponents<Touchable>().FirstOrDefault() is not { CancelSelectionOnTouch: false })
            Cancel?.Invoke(this, null);
    }
    
    public void OnDisable()
    {
        
    }
}
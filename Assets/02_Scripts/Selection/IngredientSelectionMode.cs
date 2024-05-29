using System;
using System.Linq;
using UnityEngine;

public class IngredientSelectionHandler : ISelectionHandler
{
    public static IngredientSelectionHandler Instance { get; } = new();

    public void OnEnable()
    {
        
    }

    public event EventHandler<object> Result;
    public event EventHandler Cancel;

    public void OnGameObjectTouched(GameObject @object)
    {
        var canCancelSelection = @object.GetComponents<TouchableMonoBehaviour>().FirstOrDefault() is not { CancelSelectionOnTouch: false };
        var itemProvider = @object.GetComponent<ItemProvider>() ?? GetItemProviderFromItemRenderer(@object);
        var itemProviderItem = GameSettings.GetItemMatch(itemProvider?.Item);
        var isIngredient = !itemProviderItem.Deliverable;
        
        if (isIngredient)
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

    public void OnSelectableTouched(SelectableMonoBehaviour selectable)
    {
        if (selectable.GetComponents<TouchableMonoBehaviour>().FirstOrDefault() is not { CancelSelectionOnTouch: false })
            Cancel?.Invoke(this, null);
    }
    
    public void OnDisable()
    {
        
    }
}
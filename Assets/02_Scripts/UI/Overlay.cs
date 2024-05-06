using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Overlay : SingletonMonoBehaviour<Overlay>
{
    private readonly List<ItemRenderer> _items = new();

    public void RegisterItemRenderer(ItemRenderer itemRenderer)
    {
        _items.Add(itemRenderer);
    }

    public void DestroyItemRenderer(ItemRenderer itemRenderer)
    {
        if (!itemRenderer.IsDestroyed() && !itemRenderer.Item.IsDestroyed)
            itemRenderer.Item.Destroy(); 
        
        _items.Remove(itemRenderer);
    }

    public ItemRenderer CreateItemRenderer(Item item)
    {
        var instance = Instantiate(GameSettings.Data.PRE_Item);
        var itemRenderer = instance.GetRequiredComponent<ItemRenderer>();
        instance.transform!.SetParent(gameObject.transform);
        instance.transform.localPosition = Vector3.zero;
        instance.transform.localScale = Vector3.one;
        itemRenderer.Item = item;
        item.Destroyed += (_, _) =>
        {
            Destroy(instance);
            DestroyItemRenderer(itemRenderer);
        };
        
        _items.Add(itemRenderer);
        return itemRenderer;
    }
}
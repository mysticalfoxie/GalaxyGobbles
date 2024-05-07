using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Overlay : SingletonMonoBehaviour<Overlay>
{
    public ItemRenderer CreateItemRenderer(Item item)
    {
        var instance = Instantiate(GameSettings.Data.PRE_Item);
        var itemRenderer = instance.GetRequiredComponent<ItemRenderer>();
        instance.transform!.SetParent(gameObject.transform);
        instance.transform.localPosition = Vector3.zero;
        instance.transform.localScale = Vector3.one;
        itemRenderer.Item = item;
        return itemRenderer;
    }
}
using System.Collections.Generic;
using UnityEngine;

public class Overlay : SingletonMonoBehaviour<Overlay>
{
    public Overlay() : base(true) { }

    private readonly List<ItemRenderer> _renderers = new();
    private GameObject _renderer;

    public override void Awake()
    {
        base.Awake();
        _renderer = GameSettings.Data.PRE_ItemRenderer;
    }

    public void RegisterRenderer(ItemRenderer itemRenderer)
    {
        _renderers.Add(itemRenderer);
    }

    public void DestroyRenderer(ItemRenderer itemRenderer)
    {
        _renderers.Remove(itemRenderer);
    }

    public ItemRenderer CreateItemRenderer(Item item)
    {
        var instance = Instantiate(_renderer);
        var itemRenderer = instance.GetRequiredComponent<ItemRenderer>();
        itemRenderer.Item = item;
        item.Destroyed += (_, _) =>
        {
            Destroy(instance);
            DestroyRenderer(itemRenderer);
        };
        
        _renderers.Add(itemRenderer);
        return itemRenderer;
    }
}
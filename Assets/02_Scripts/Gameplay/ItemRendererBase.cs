using System.Linq;
using UnityEngine;

public class ItemRendererBase : TouchableMonoBehaviour
{
    protected SpriteRenderer _renderer;

    protected ItemData ItemData { get; private set; }
    protected Item Item { get; private set; }

    public override void Awake()
    { 
        _renderer = this
            .GetChildren()
            .Select(x => x.GetComponent<SpriteRenderer>())
            .First(x => x is not null);
        
        base.Awake();
    }

    protected void RenderItem(ItemData item)
    {
        ItemData = item;
        _renderer.sprite = ItemData.Sprite;
    }

    protected void RenderItem(Item item)
    {
        Item = item;
        ItemData = item.Data;
        _renderer.sprite = ItemData.Sprite;
    }
}
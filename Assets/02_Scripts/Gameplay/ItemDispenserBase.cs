using System.Linq;
using UnityEngine;

public class ItemDispenserBase : TouchableMonoBehaviour
{
    private SpriteRenderer _renderer;

    protected ItemData Item { get; private set; }

    public override void Awake()
    { 
        _renderer = this
            .GetChildren()
            .Select(x => x.GetComponent<SpriteRenderer>())
            .First(x => x is not null);
        
        base.Awake();
    }

    protected void SetItem(ItemData item)
    {
        Item = item;
        _renderer.sprite = Item.Sprite;
    }
}
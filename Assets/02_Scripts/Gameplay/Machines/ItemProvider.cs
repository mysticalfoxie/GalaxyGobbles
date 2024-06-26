using System;
using UnityEngine;

public class ItemProvider : Touchable
{
    [Header("Item Data")] 
    [SerializeField] private ItemData _item;
    [SerializeField] private Vector2 _offset;
    
    private Item _itemCache;
    public ItemData Item => _item;

    public override void Awake()
    {
        base.Awake();
        
        var item = GameSettings.GetItemMatch(_item);
        _itemCache = new Item(this, item, true);
        _itemCache.Follow(this, _offset);
        _itemCache.ForwardTouchEventsTo(this);
    } 

    private void OnValidate()
    {
        if (gameObject.scene == default) return; // Exclude validation of Prefabs
        if (_item is null) throw new ArgumentNullException($"The item of item provider \"{name}\" is empty!");
        GameSettings.GetItemMatch(_item);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.HSVToRGB(0F, .7F, .7F);
        Gizmos.DrawCube(transform.position, transform.lossyScale);
    }

    protected override void OnTouch()
    {
        var newItem = _itemCache.Clone();
        if (!BottomBar.Instance.Inventory.TryAdd(newItem))
            newItem.Dispose();
        else
            newItem.Show();
    }
}
using System;
using UnityEngine;

public class ItemProvider : TouchableMonoBehaviour
{
    [Header("Item Visualization")] [SerializeField] private Vector2 _offset;
    [Header("Provider Data")] [SerializeField] private ItemData _itemData;
    
    private Item _item;

    public override void Awake()
    {
        if (_itemData is null) throw new ArgumentNullException($"The Component {nameof(ItemProvider)} has a field {nameof(_itemData)} that has to be assigned.");
        
        base.Awake();
        _item = new Item(_itemData, true);
        _item.AlignTo(this, _offset);
        _item.ForwardTouchEventsTo(this);
    }
    
    protected override void OnTouch()
    {
        var newItem = _item.Clone();
        if (!Sidebar.Instance.Inventory.TryAdd(newItem))
            newItem.Dispose();
    }
}
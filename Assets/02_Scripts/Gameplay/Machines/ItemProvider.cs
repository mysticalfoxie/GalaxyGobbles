using System;
using UnityEngine;

public class ItemProvider : TouchableMonoBehaviour
{
    [Header("Item Data")] 
    [SerializeField] private string _itemId = Identifiers.EMPTY_STRING;
    [SerializeField] private Vector2 _offset;
    
    private Item _item;
    
    public string ItemId => _itemId;

    public override void Awake()
    {
        base.Awake();
        
        var item = GameSettings.GetItemById(_itemId);
        _item = new Item(this, item, true);
        _item.AlignTo(this, _offset);
        _item.ForwardTouchEventsTo(this);
    }

    private void OnValidate()
    {
        if (_itemId is null) throw new ArgumentNullException($"The field \"{nameof(_itemId)}\" of component \"{nameof(ItemProvider)}\" is not assigned.");
        GameSettings.GetItemById(_itemId);
    }

    protected override void OnTouch()
    {
        var newItem = _item.Clone();
        if (!BottomBar.Instance.Inventory.TryAdd(newItem))
            newItem.Dispose();
        else
            newItem.Show();
    }
}
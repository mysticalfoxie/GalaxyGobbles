using UnityEngine;

public class TrashCan : TouchableMonoBehaviour
{
    [Header("Item Visualization")]
    [SerializeField] private ItemData _itemData;
    [SerializeField] private Vector2 _offset;
    
    private Item _item;

    public override void Awake()
    {        
        base.Awake();

        _item = new Item(_itemData, true);
        _item.AlignTo(this, _offset);
        _item.ForwardTouchEventsTo(this);
    }

    protected override void OnTouch()
    {
        BottomBar.Instance.Inventory.Reset();
    }
}
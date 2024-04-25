using UnityEngine;

public class ItemDispenser : ItemDispenserBase
{
    [Header("Provider Data")]
    [SerializeField]
    private ItemData _item;

    public override void Awake()
    {
        base.Awake();
        SetItem(_item);
    }
    
    protected override void OnTouch()
    {
        Sidebar.Instance.Inventory.Create(Item);
    }
}
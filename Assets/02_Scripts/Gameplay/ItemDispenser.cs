using UnityEngine;

public class ItemDispenser : ItemRendererBase
{
    [Header("Provider Data")]
    [SerializeField]
    private ItemData _item;

    public override void Awake()
    {
        base.Awake();
        RenderItem(_item);
    }
    
    protected override void OnTouch()
    {
        Sidebar.Instance.Inventory.Create(ItemData);
    }
}
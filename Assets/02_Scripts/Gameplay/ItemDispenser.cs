using UnityEngine;

public class ItemDispenser : SpriteRendererBase
{
    [Header("Provider Data")]
    [SerializeField]
    private ItemData _item;
    
    protected ItemData ItemData { get; private set; }

    public override void Awake()
    {
        base.Awake();
        RenderSprite(_item);
    }
    
    protected override void OnTouch()
    {
        Sidebar.Instance.Inventory.Create(ItemData);
    }
}
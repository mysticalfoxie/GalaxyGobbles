using System;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

public class ItemProvider : Touchable
{
    [Header("Item Data")] 
    [SerializeField] private ItemData _item;
    [SerializeField] private RectTransform _canvas;
    [SerializeField] [Range(0.1F, 5.0F)] private float _scale = 1;
    
    private Item _itemCache;
    public ItemData Item => _item;

    public override void Awake()
    {
        base.Awake();
        
        var item = GameSettings.GetItemMatch(_item);
        _itemCache = new Item(new(this, item, true, ItemDisplayDimension.Dimension3D))
            .ForwardTouchEventsTo(this)
            .SetParent(_canvas.transform)
            .SetLocalPosition(Vector2.zero)
            .SetRotation(Vector2.zero)
            .SetScale(new Vector2(_scale, _scale));
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
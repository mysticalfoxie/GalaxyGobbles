using UnityEngine;

public class NoodleBowl : Touchable
{
    private Item _item;
    private ItemData _itemData;

    [Header("Item Visualization")] [SerializeField] private Vector2 _itemOffset;

    public override void Awake()
    {
        base.Awake();

        _itemData = GameSettings.GetItemMatch(Identifiers.Value.NoodleBowl);
        _item = new Item(this, _itemData, true);
        _item.AlignTo(this, _itemOffset);
        _item.ForwardTouchEventsTo(this);
    }

    protected override void OnTouch()
    {
        NoodlePotDistributor.AddNoodles();
    }
}
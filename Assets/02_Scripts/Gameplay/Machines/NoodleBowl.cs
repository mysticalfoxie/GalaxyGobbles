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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.HSVToRGB(.1F, .7F, .7F);
        Gizmos.DrawCube(transform.position, transform.lossyScale);
    }

    protected override void OnTouch()
    {
        NoodlePotDistributor.AddNoodles();
    }
}
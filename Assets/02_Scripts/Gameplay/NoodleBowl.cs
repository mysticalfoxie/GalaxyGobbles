using System.Linq;
using UnityEngine;

public class NoodleBowl : TouchableMonoBehaviour
{
    private Item _item;
    private ItemData _itemData;

    [Header("Item Visualization")] [SerializeField] private Vector2 _itemOffset;

    public override void Awake()
    {
        base.Awake();

        _itemData = GameSettings.Data.Items.First(x => x.Id == ItemId.ID_01_NoodleBowl);
        _item = new Item(_itemData);
        _item.AlignTo(this, _itemOffset);
        _item.ForwardClickEventsTo(this);
    }

    protected override void OnTouch()
    {
        NoodlePotDistributor.AddNoodles();
    }
}
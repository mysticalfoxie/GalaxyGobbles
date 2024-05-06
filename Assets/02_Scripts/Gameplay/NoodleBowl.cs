using System.Linq;

public class NoodleBowl : TouchableMonoBehaviour
{
    private Item _item;
    private ItemData _itemData;

    public override void Awake()
    {
        base.Awake();

        _itemData = GameSettings.Data.Items.First(x => x.Id == ItemId.ID_01_NoodleBowl);
        _item = new Item(_itemData);
        _item.AlignTo(this);
    }
}
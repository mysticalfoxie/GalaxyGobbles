using UnityEngine;

public class ItemSummoner : MonoBehaviour
{
    [SerializeField] private ItemData _itemData;
    [SerializeField] private GameObject _alignTo;
    private Item _item;

    public void Start()
    {
        _item = new Item(this, _itemData, true);
        _item.AlignTo(_alignTo);
    }
}
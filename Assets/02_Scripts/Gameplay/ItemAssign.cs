using UnityEngine;

public class ItemAssign : MonoBehaviour
{
    private Item _item;

    public void Awake()
    {
        _item = this.GetRequiredComponent<Item>();
        _item.Data = References.Instance.Items.Noodles;
    }
}
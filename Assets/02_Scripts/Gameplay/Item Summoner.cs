using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Item_Summoner : MonoBehaviour
{
    [SerializeField] private ItemData _itemData;
    [SerializeField] private GameObject _alignTo;
    private Item _item;

    public void Start()
    {
        _item = new Item(_itemData);
        _item.AlignTo(_alignTo);
        StartCoroutine(nameof(OnOff));
    }

    private IEnumerator OnOff()
    {
        while (!gameObject.IsDestroyed())
        {
            yield return new WaitForSeconds(5);
            if (_item.Hidden) _item.Show();
            else _item.Hide();
        }
    }
}
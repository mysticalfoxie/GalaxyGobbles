using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    private Sprite _sprite;
    private SpriteRenderer _renderer;
    private ItemData _data;
    
    public ItemData Data
    {
        get => _data;
        set => UpdateData(value);
    }

    public void Awake()
    {
        _renderer = this.GetRequiredComponent<SpriteRenderer>();
        _renderer.sprite = Data.Sprite;
    }

    private void UpdateData(ItemData data)
    {
        _data = data ? data : throw new ArgumentNullException(nameof(data));
        _renderer.sprite = Data.Sprite;
    }

    public static Item Create(ItemData itemData)
    {
        var prefab = References.Instance.ItemPrefab;
        var instance = Instantiate(prefab);
        var item = instance.GetRequiredComponent<Item>();
        item.Data = itemData;
        return item;
    }
}
using System;
using UnityEngine;

public class Item : ItemRendererBase
{
    private Sprite _sprite;
    private SpriteRenderer _renderer;
    private ItemData _data;
    
    public ItemData Data
    {
        get => _data;
        protected set => UpdateData(value);
    }

    private void UpdateData(ItemData data)
    {
        _data = data ? data : throw new ArgumentNullException(nameof(data));
        _renderer.sprite = Data.Sprite;
        RenderItem(this);
    }

    public static Item Create(ItemData itemData)
    {
        var prefab = ReferencesSettings.Data.ItemPrefab;
        var instance = Instantiate(prefab);
        var item = instance.GetRequiredComponent<Item>();
        item.Data = itemData;
        return item;
    }
}
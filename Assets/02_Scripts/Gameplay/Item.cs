using System;

public class Item : TouchableMonoBehaviour
{
    private ItemData _data;
    
    public ItemData Data
    {
        get => _data;
        set => UpdateData(value);
    }

    private void UpdateData(ItemData data)
    {
        _data = data ? data : throw new ArgumentNullException(nameof(data));
        //RenderSprite(this);
    }

    public static Item Create(ItemData itemData)
    {
        var prefab = ReferencesSettings.Data.ItemPrefab;
        var instance = Instantiate(prefab);
        var item = instance.GetRequiredComponent<Item>();
        item.gameObject.SetActive(false);
        item.Data = itemData;
        return item;
    }
}
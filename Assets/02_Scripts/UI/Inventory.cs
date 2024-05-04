using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private readonly List<Item> _items = new();
    private Image[] _renderers;
    
    public void Awake()
    {
        _renderers = this
            .GetChildren()
            .Select(x => x.GetComponent<Image>())
            .Where(x => x is not null)
            .ToArray();
    }


    public void Add(Item item)
    {
        if (IsFull()) return;
        
        _items.Add(item);
        RefreshView();
    }

    public bool HasItem(Item item) => _items.Any(x => x == item);

    public bool HasItem(ItemData item) => false; // _items.Any(x => x.Data == item);

    public bool IsFull() => _items.Count >= _renderers.Length;

    public void Remove(Item item)
    {
        _items.Remove(item);
        RefreshView();
    }

    public void RefreshView()
    {
        foreach (var spriteRenderer in _renderers)
            spriteRenderer.sprite = null;

        for (var i = 0; i < _renderers.Length; i++)
            _renderers[i].sprite = i < _items.Count ? null : null; // _items[i].Data.Sprite : null;
    }

    public void Reset()
    {
        _items.Clear();
        RefreshView();
    }

    public Item GetItemOfType(ItemData data)
    {
        return null; // TODO: GetItemOfType
                     // _items.FirstOrDefault(x => x.Data == data);
    }

    public void Create(ItemData itemData)
    {
        if (IsFull()) return;

        // var item = Item.Create(itemData);
        // _items.Add(item);
        // RefreshView();
    }
}


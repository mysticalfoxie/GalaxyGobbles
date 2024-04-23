using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private readonly List<ItemData> _items = new();
    private Image[] _renderers;
    
    public void Awake()
    {
        _renderers = this
            .GetChildren()
            .Select(x => x.GetComponent<Image>())
            .Where(x => x is not null)
            .ToArray();
    }

    public void Add(ItemData item)
    {
        if (IsFull()) return;
        
        _items.Add(item);
        RefreshView();
    }

    public bool HasItem(ItemData item) => _items.Any(x => x == item);

    public bool IsFull() => _items.Count >= _renderers.Length;

    public void Remove(ItemData item)
    {
        _items.Remove(item);
        RefreshView();
    }

    public void RefreshView()
    {
        foreach (var spriteRenderer in _renderers)
            spriteRenderer.sprite = null;

        for (var i = 0; i < _renderers.Length; i++)
            _renderers[i].sprite = i < _items.Count ? _items[i].Sprite : null;
    }

    public void Clear()
    {
        _items.Clear();
        RefreshView();
    }
}

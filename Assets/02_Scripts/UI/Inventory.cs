using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private readonly List<Item> _items = new();
    private GameObject[] _positions;
    
    public void Awake()
    {
        _positions = this.GetChildren().ToArray();
    }


    public void Add(Item item)
    {
        if (IsFull()) return;
        
        _items.Add(item);
        RefreshView();
    }

    public bool IsFull() => _items.Count >= _positions.Length;

    public void Remove(Item item, bool removeWithoutDestroy = false)
    {
        if (removeWithoutDestroy) 
            item.Destroy();
        
        _items.Remove(item);
        RefreshView();
    }

    public void RefreshView()
    {
        for (var index = 0; index < _items.Count; index++)
        {
            var item = _items[index];
            item.AlignTo(_positions[index].gameObject);
        }
    }

    public void Reset()
    {
        foreach (var item in _items)
            item.Destroy();
        
        _items.Clear();
        RefreshView();
    }
}


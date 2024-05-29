using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private readonly List<Item> _items = new();
    private GameObject[] _positions;
    
    public void Awake()
    {
        _positions = this.GetChildren().ToArray();
    }
    
    public bool TryAdd(Item item)
    {
        if (TryCraftSomethingWith(item)) return false;
        if (!item.Data.Deliverable) return false;
        if (IsFull()) return false;
        
        _items.Add(item);
        RefreshView();
        return true;
    }

    public bool IsFull() => _items.Count >= _positions.Length;

    private bool TryCraftSomethingWith(Item item)
    {
        var recipe = _items.GetCraftableRecipesWith(item).FirstOrDefault(x => x.IsMatch);
        if (!recipe.IsMatch) return false;
        var newItem = recipe.Fulfill();
        Replace(recipe.ItemA, newItem);
        return true;
    }

    public bool TryRemove(ItemData value, bool destroy = false)
    {
        var item = _items.FirstOrDefault(x => x.Data.name == value.name);
        if (item is null) return false;
        if (destroy) item.Dispose();
        _items.Remove(item);
        RefreshView();
        return true;
    }    

    public void Reset()
    {
        foreach (var item in _items)
            item.Dispose();
        
        _items.Clear();
        RefreshView();
    }
    
    private void Replace(Item oldItem, Item newItem)
    {
        var index = _items.IndexOf(oldItem);
        _items[index] = newItem;
    }
    
    private void RefreshView()
    {
        for (var index = 0; index < _items.Count; index++)
        {
            var item = _items[index];
            item.AlignTo(_positions[index].gameObject);
        }
    }
}


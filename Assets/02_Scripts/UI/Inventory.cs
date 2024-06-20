using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private readonly List<Item> _items = new();
    private GameObject[] _positions;

    public IEnumerable<Item> Items => _items;

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

    public void Remove(Item value, bool destroy = false)
    {
        var item = _items.First(x => x.Data.name == value.Data.name);
        if (destroy) item.Dispose();
        _items.Remove(item);
        RefreshView();
    }

    public void Reset()
    {
        foreach (var item in _items)
            item.Dispose();
        
        _items.Clear();
        RefreshView();
    }

    public void AddPoison(ItemData data)
    {
        if (!data.CanBecomePoison) return;
        
        // First item that is not poisoned yet.
        var item = _items.FirstOrDefault(x => !x.Data.Poison && x.Data.CanBecomePoisoned); 
        if (item is null) return;
        
        // Poison it
        item.Data.Poison = data;
        item.Refresh();
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
            item.Follow(_positions[index].gameObject);
        }
    }
}


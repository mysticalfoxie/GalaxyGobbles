using System;
using UnityEngine;
 
public class Item
{
    private ItemRenderer _renderer;

    public Item(ItemData data, bool hideOnCreation = false)
    {
        Data = data;
        if (!hideOnCreation) Show();
    }

    private ItemData Data { get; }
    public event EventHandler Destroyed;

    public void Show()
    {
        Render();
    }

    public void Hide()
    {
        _renderer.Disable();
    }

    public void Combine(Item item)
    {
        // TODO: Crafting REZEPTE 
    }
    
    public void AlignTo(GameObject value, Vector2 offset = default)
    {
        var offset3d = new Vector3(offset.x, offset.y, 0);
        var position = value.gameObject.transform.position + offset3d;
        var screen = LevelManager.Instance.Camera.WorldToScreenPoint(position);
        
    }

    public void Follow(GameObject value, Vector2 offset = default)
    {
        
    } 

    public void Destroy()
    {
        Destroyed?.Invoke(this, EventArgs.Empty);
    }

    private void Render()
    {
        _renderer ??= Overlay.Instance.CreateItemRenderer(this);
        if (_renderer.Disabled) _renderer.Enable();
    }
}
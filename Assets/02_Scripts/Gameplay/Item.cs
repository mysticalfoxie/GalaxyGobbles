using System;
using UnityEngine;
 
public class Item
{
    private readonly ItemRenderer _renderer;

    public Item(ItemData data, bool hideOnCreation = false)
    {
        Data = data;
        _renderer = CreateItemRenderer();
        if (hideOnCreation) 
            Hide();
        else 
            Show();
    }

    public ItemData Data { get; }
    public bool IsDestroyed { get; private set; }
    public bool Hidden { get; private set; }
    public event EventHandler Destroyed;
    public event EventHandler Click;

    public void Show()
    {
        Render();
        Hidden = false;
    }

    public void Hide()
    {
        _renderer.Disable();
        Hidden = true;
    }

    public void Combine(Item item)
    {
        // TODO: Crafting REZEPTE 
    }

    public void AlignTo(MonoBehaviour value, Vector2 offset = default) => AlignTo(value.gameObject, offset);
    public void AlignTo(GameObject value, Vector2 offset = default)
    {
        if (_renderer is null) return;
        _renderer.AlignTo(value, offset);
    }
    
    public void Follow(MonoBehaviour value, Vector2 offset = default) => Follow(value.gameObject, offset);
    public void Follow(GameObject value, Vector2 offset = default)
    {
        if (_renderer is null) return;
        _renderer.Follow(value, offset);
    }

    public void ForwardClickEventsTo(TouchableMonoBehaviour touchable)
    {
        Click += touchable.InvokeTouch;
    }

    public void Destroy()
    {
        Destroyed?.Invoke(this, EventArgs.Empty);
        IsDestroyed = true;
    }

    private void Render()
    {
        if (_renderer.Disabled) 
            _renderer.Enable();
    }

    private ItemRenderer CreateItemRenderer()
    {
        var itemRenderer = Overlay.Instance.CreateItemRenderer(this);
        itemRenderer.Item = this;
        itemRenderer.Click += (o, e) => Click?.Invoke(o, e);
        return itemRenderer;
    }
}
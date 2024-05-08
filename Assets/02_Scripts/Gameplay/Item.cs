using System;
using UnityEngine;
 
public class Item : IDisposable
{
    private ItemRenderer _renderer;
    private readonly bool _initialized;

    public Item(ItemData data, bool renderItemOnCreation = false)
    {
        Data = data;
        if (renderItemOnCreation) Show();
        else Hidden = true;
        _initialized = true; 
    }

    public ItemData Data { get; }
    public GameObject AlignedTo { get; private set; }
    public GameObject Following { get; private set; }
    public Vector2 AlignmentOffset { get; private set; }
    public Vector2 FollowOffset { get; private set; }
    public bool Hidden { get; private set; }
    public event EventHandler Click;

    public void Show()
    {
        if (!Hidden && _initialized) return;
        _renderer = CreateItemRenderer();
        if (AlignedTo.IsAssigned()) _renderer.AlignTo(AlignedTo, AlignmentOffset);
        if (Following.IsAssigned()) _renderer.Follow(Following, FollowOffset);
        Hidden = false;
    }

    public void Hide()
    {
        if (Hidden) return;
        _renderer.Destroy();
        Hidden = true;
    }

    public void AlignTo(Item value, Vector2 offset = default) => AlignTo(value._renderer?.gameObject, offset);
    public void AlignTo(MonoBehaviour value, Vector2 offset = default) => AlignTo(value.gameObject, offset);
    public void AlignTo(GameObject value, Vector2 offset = default)
    {
        if (!value.IsAssigned()) return; 
        AlignedTo = value;
        AlignmentOffset = offset;
        if (_renderer.IsDestroyed()) return;
        _renderer.AlignTo(value, offset);
    }
    
    public void Follow(Item value, Vector2 offset = default) => Follow(value._renderer?.gameObject, offset);
    public void Follow(MonoBehaviour value, Vector2 offset = default) => Follow(value.gameObject, offset);
    public void Follow(GameObject value, Vector2 offset = default)
    {
        if (!value.IsAssigned()) return; 
        Following = value;
        FollowOffset = offset;
        if (_renderer.IsDestroyed()) return;
        _renderer.AlignTo(value, offset);
        _renderer.Follow(value, offset);
    }

    public void ForwardTouchEventsTo(TouchableMonoBehaviour touchable)
    {
        Click += touchable.InvokeTouch;
    }

    private ItemRenderer CreateItemRenderer()
    {
        var itemRenderer = Overlay.Instance.CreateItemRenderer(this);
        itemRenderer.Item = this;
        itemRenderer.Click += (o, e) => Click?.Invoke(o, e);
        return itemRenderer;
    }

    public Item Clone(bool showOnCreation = false)
    {
        return new Item(Data.Clone(), showOnCreation);
    }

    public void Dispose()
    {
        if (_renderer is not null && !_renderer.Destroyed) _renderer.Destroy();
        
        GC.SuppressFinalize(this);
    }
}
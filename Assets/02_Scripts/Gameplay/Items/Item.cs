using System;
using UnityEngine;
 
public class Item : IDisposable
{
    private ItemRenderer _renderer;
    private readonly bool _initialized;

    public Item(object initiator, ItemData data, bool renderItemOnCreation = false)
    {
        Initiator = initiator;
        Data = data;
        if (renderItemOnCreation) Show();
        else Hidden = true;
        _initialized = true; 
    }

    public object Initiator { get; }
    public ItemData Data { get; }
    public GameObject AlignedTo { get; private set; }
    public GameObject Following { get; private set; }
    public Vector2 AlignmentOffset { get; private set; }
    public Vector2 FollowOffset { get; private set; }
    public bool Hidden { get; private set; }
    public event EventHandler Click;

    public Item Show()
    {
        if (!Hidden && _initialized) return this;
        _renderer = CreateItemRenderer();
        if (AlignedTo.IsAssigned()) _renderer.AlignTo(AlignedTo, AlignmentOffset);
        if (Following.IsAssigned()) _renderer.Follow(Following, FollowOffset);
        Hidden = false;
        return this;
    }

    public Item Hide()
    {
        if (Hidden) return this;
        _renderer.Destroy();
        Hidden = true;
        return this;
    }

    public Item AlignTo(Item value, Vector2 offset = default) => AlignTo(value._renderer?.gameObject, offset);
    public Item AlignTo(MonoBehaviour value, Vector2 offset = default) => AlignTo(value.gameObject, offset);
    public Item AlignTo(GameObject value, Vector2 offset = default)
    {
        if (!value.IsAssigned()) return this; 
        AlignedTo = value;
        AlignmentOffset = offset;
        if (_renderer.IsDestroyed()) return this;
        _renderer.AlignTo(value, offset);
        return this;
    }
    
    public Item Follow(Item value, Vector2 offset = default) => Follow(value._renderer?.gameObject, offset);
    public Item Follow(MonoBehaviour value, Vector2 offset = default) => Follow(value.gameObject, offset);
    public Item Follow(GameObject value, Vector2 offset = default)
    {
        if (!value.IsAssigned()) return this; 
        Following = value;
        FollowOffset = offset;
        if (_renderer.IsDestroyed()) return this;
        _renderer.AlignTo(value, offset);
        _renderer.Follow(value, offset);
        return this;
    }

    public Item StopFollowing()
    {
        _renderer.StopFollowing();
        return this;
    }

    public Item SendToBack()
    {
        _renderer.SendToBack();
        return this;
    }

    public Item SendToFront()
    {
        _renderer.SendToFront();
        return this;
    }

    public Item ForwardTouchEventsTo(TouchableMonoBehaviour touchable)
    {
        Click += (_, _) => touchable?.InvokeTouch(this, EventArgs.Empty);
        return this;
    }

    public Item Clone(bool showOnCreation = false)
    {
        return new Item(this, Data.Clone(), showOnCreation);
    }

    public void Dispose()
    {
        if (_renderer is not null && !_renderer.Destroyed) 
            _renderer.Destroy();
        
        GC.SuppressFinalize(this);
    }

    private ItemRenderer CreateItemRenderer()
    {
        var itemRenderer = Overlay.Instance.CreateItemRenderer(this, Initiator);
        itemRenderer.Item = this;
        itemRenderer.Click += (o, e) => Click?.Invoke(o, e);
        return itemRenderer;
    }
}
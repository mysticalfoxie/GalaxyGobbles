using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

// Attached to the item Game Object. Controls the children. 
public class ItemRenderer : TouchableMonoBehaviour
{
    private readonly Dictionary<GameObject, Image> _renderers = new();
    private GameObject _follow;
    private Vector2 _followOffset;
    private SpriteData[] _sprites;
    private Item _item;

    public bool Destroyed { get; private set; }

    public event EventHandler OnDestroyed; 
    
    public Item Item
    {
        get => _item;
        set => UpdateItem(value);
    }

    public bool Disabled { get; private set; }
    public event EventHandler Click;

    public void Update()
    {
        if (_follow.IsAssigned(() => _follow = null)) 
            AlignTo(_follow, _followOffset);
    }
    
    public void AlignTo(GameObject value, Vector2 offset = default)
    {
        if (value.layer == LayerMask.NameToLayer("UI"))
            AlignToUIObject(value, offset);
        else
            AlignTo3DObject(value, offset);
    }

    public void Follow(GameObject value, Vector2 offset = default)
    {
        _follow = value;
        _followOffset = offset;
    }

    public void StopFollowing()
    {
        _follow = null;
        _followOffset = default;
    }
    
    protected override void OnTouch()
    {
        Click?.Invoke(this, EventArgs.Empty);
    }

    protected void AddSprite(SpriteData sprite)
    {
        var rendererGameObject = Instantiate(GameSettings.Data.PRE_SpriteRenderer);
        var rendererComponent = rendererGameObject.GetRequiredComponent<Image>();
        rendererComponent.transform.SetParent(gameObject.transform);
        rendererComponent.sprite = sprite.Sprite;
        rendererComponent.rectTransform.sizeDelta = sprite.Size == default ? sprite.Sprite.texture.Size() : sprite.Size;
        rendererComponent.rectTransform.transform.localPosition = sprite.Offset;
        rendererComponent.rectTransform.transform.localScale = Vector2.one;
        rendererComponent.rectTransform.transform.localScale = Vector2.one;
        rendererComponent.rectTransform.transform.localRotation = Quaternion.Euler(0F, 0F, sprite.Rotation);
        _renderers.Add(rendererGameObject, rendererComponent);
    }

    protected void RemoveSprite(SpriteData sprite)
    {
        var (rendererGameObject, _) = _renderers.First(x => x.Value.sprite == sprite.Sprite);
        _renderers.Remove(rendererGameObject);
        Destroy(rendererGameObject);
    }

    private void AlignToUIObject(GameObject value, Vector2 offset = default)
    {
        var offset3d = new Vector3(offset.x, offset.y, 0);
        var position = value.transform.position;
        gameObject.transform.position = position + offset3d;
    }

    private void AlignTo3DObject(GameObject value, Vector2 offset = default)
    {
        var offset3d = new Vector3(offset.x, offset.y, 0);
        var position = value.gameObject.transform.position;
        var screen = Raycaster.Instance.Camera.WorldToScreenPoint(position);
        gameObject.transform.position = screen + offset3d;
    }

    private void DetectChanges(SpriteData[] newSprites, out SpriteData[] removed, out SpriteData[] added)
    {
        added = newSprites
            .Where(x => x is not null)
            .Where(x => !_sprites?.Contains(x) ?? true)
            .ToArray();
        
        removed = _sprites?
            .Where(x => !newSprites.Contains(x))
            .ToArray() ?? Array.Empty<SpriteData>();
    }

    private void UpdateItem(Item item)
    {
        _item = item ?? throw new ArgumentNullException(nameof(item));
        var sprites = _item.Data.Sprites.ToArray();
        DetectChanges(sprites, out var removed, out var added);
        foreach (var removedSprite in removed) RemoveSprite(removedSprite);
        foreach (var addedSprite in added) AddSprite(addedSprite);
        IndexingSprites(); // Experimental... Don't know if it works with unity's "SetSiblingIndex"
        _sprites = sprites;
    }

    private void IndexingSprites()
    {
        if (_sprites is null) return;
        
        var spriteRenderers = _sprites
            .OrderBy(x => x.Order)
            .ToDictionary(x => x, x => _renderers.First(y => y.Value.sprite == x.Sprite).Value);
        
        for (var i = 0; i < spriteRenderers.Count; i++)
        {
            var image = spriteRenderers.ElementAt(i).Value;
            image.transform.SetSiblingIndex(i);
        }
    }

    public void Destroy()
    {
        if (Destroyed) return;
        Destroyed = true;
        OnDestroyed?.Invoke(this, EventArgs.Empty);
        Destroy(gameObject);
    }
}
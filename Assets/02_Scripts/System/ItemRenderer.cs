using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class ItemRenderer : TouchableMonoBehaviour
{
    private List<Image> _renderers;
    private GameObject _renderer;
    private GameObject _follow;
    private Vector2 _followOffset;
    private SpriteData[] _sprites;

    public Item Item
    {
        get;
        set;
    }
    
    public SpriteData[] Sprites
    {
        get => _sprites;
        set => UpdateSprites(value);
    }

    public bool Disabled { get; private set; }
    
    public override void Awake()
    {
        base.Awake();
        _renderer = GameSettings.Data.PRE_ItemRenderer;
        Overlay.Instance.RegisterRenderer(this);
    }

    public void Update()
    {
        if (_follow is not null) AlignTo(_follow, _followOffset);
    }

    public void Disable()
    {
        Disabled = true;
    }

    public void Enable()
    {
        Disabled = false;
    }
    
    public void AlignTo(GameObject value, Vector2 offset = default)
    {
        var offset3d = new Vector3(offset.x, offset.y, 0);
        var position = value.gameObject.transform.position + offset3d;
        var screen = LevelManager.Instance.Camera.WorldToScreenPoint(position);
        gameObject.transform.position = screen;
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
    
    protected void AddSprite(SpriteData sprite)
    {
        var rendererGameObject = Instantiate(_renderer);
        var rendererComponent = rendererGameObject.GetRequiredComponent<Image>();
        rendererComponent.transform.SetParent(gameObject.transform);
        rendererComponent.sprite = sprite.Sprite;
        rendererComponent.transform.position = sprite.Offset.GetValueOrDefault();
        _renderers.Add(rendererComponent);
    }

    protected void RemoveSprite(SpriteData sprite)
    {
        var rendererComponent = _renderers.First(x => x.sprite == sprite.Sprite);
        _renderers.Remove(rendererComponent);
        Destroy(rendererComponent.gameObject);
    }

    private void DetectChanges(SpriteData[] newSprites, out SpriteData[] removed, out SpriteData[] added)
    {
        added = newSprites.Where(x => !_sprites.Contains(x)).ToArray();
        removed = _sprites.Where(x => !newSprites.Contains(x)).ToArray();
    }

    private void UpdateSprites(SpriteData[] sprites)
    {
        DetectChanges(sprites, out var removed, out var added);
        foreach (var removedSprite in removed) RemoveSprite(removedSprite);
        foreach (var addedSprite in added) AddSprite(addedSprite);
        IndexingSprites(); // Experimental... Don't know if it works with unity's "SetSiblingIndex"
        _sprites = sprites;
    }

    private void IndexingSprites()
    {
        var spriteRenderers = _sprites
            .OrderBy(x => x.Order)
            .ToDictionary(x => x, x => _renderers.First(y => y.sprite == x.Sprite));
        
        for (var i = 0; i < spriteRenderers.Count; i++)
        {
            var image = spriteRenderers.ElementAt(i).Value;
            image.transform.SetSiblingIndex(i);
        }
    }

    private void OnDestroy()
    {
        Overlay.Instance.DestroyRenderer(this);
    }
}
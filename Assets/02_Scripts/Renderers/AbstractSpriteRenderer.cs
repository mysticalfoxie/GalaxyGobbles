using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class AbstractSpriteRenderer : TouchableMonoBehaviour
{
    private List<SpriteRenderer> _renderers;
    private GameObject _renderer;
    
    public override void Awake()
    {
        base.Awake();
        _renderer = References.Instance.ReferenceSettings.SpriteRenderer;
    }

    protected void AddSprite(SpriteData sprite)
    {
        var rendererGameObject = Instantiate(_renderer);
        var rendererComponent = rendererGameObject.GetRequiredComponent<SpriteRenderer>();
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
}
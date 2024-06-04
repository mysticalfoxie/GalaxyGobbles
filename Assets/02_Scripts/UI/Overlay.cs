using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Overlay : SingletonMonoBehaviour<Overlay>
{
    private readonly List<ItemRenderer> _renderers = new();
    
    public override void Awake()
    {
        base.Awake();

        InheritedDDoL = true;
    }

    public ItemRenderer CreateItemRenderer(Item item, object initiator)
    {
        var instance = Instantiate(GameSettings.Data.PRE_Item);
        var itemRenderer = instance.GetRequiredComponent<ItemRenderer>();
        itemRenderer.Initiator = initiator;
        instance.transform!.SetParent(gameObject.transform);
        instance.transform.localPosition = Vector3.zero;
        instance.transform.localScale = Vector3.one;
        itemRenderer.Item = item;
        _renderers.Add(itemRenderer);
        itemRenderer.OnDestroyed += (_, _) 
            => _renderers.Remove(itemRenderer);
        
        return itemRenderer;
    }

    protected override void OnSceneUnloaded(Scene scene)
    {
        var itemRenderers = _renderers.Where(x => !x.IsDestroyed()).ToArray();
        foreach (var itemRenderer in itemRenderers)
        {
            itemRenderer.gameObject.SetActive(false);
            itemRenderer.enabled = false;
            itemRenderer.Destroy();
        }
    }
}
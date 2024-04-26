using System.Collections;
using UnityEngine;

public class NoodlePot : ItemRendererBase
{
    private bool _cooking;
    
    public Noodles Noodles { get; private set; }
    
    protected override void OnTouch()
    {
        
    }

    public void SetNoodles(Noodles noodles)
    {
        if (Noodles is not null) return;
        if (_cooking) return;
        
        Noodles = noodles;
        RenderItem(Noodles);
        _cooking = true;

        StartCoroutine(nameof(OnCookingStart));
    }

    private IEnumerator OnCookingStart()
    {
        yield return new WaitForSeconds(GeneralSettings.Data.NoodleBoilingTime);
        
        
    }
}
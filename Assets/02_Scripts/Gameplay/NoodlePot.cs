using System.Collections;
using UnityEngine;

public class NoodlePot : TouchableMonoBehaviour
{
    private bool _cooking;
    
    public NoodleData Noodles { get; private set; }
    
    protected override void OnTouch()
    {
        
    }

    public void SetNoodles(NoodleData noodles)
    {
        if (Noodles is not null) return;
        if (_cooking) return;
        
        Noodles = noodles;
        //RenderSprite(Noodles);
        _cooking = true;

        StartCoroutine(nameof(OnCookingStart));
    }

    private IEnumerator OnCookingStart()
    {
        var boilingTime = GameSettings.Data.NoodleBoilingTime;
        yield return new WaitForSeconds(boilingTime);
        
    }
}
using UnityEngine;

public class NoodlePot : ItemRendererBase
{
    private bool _cooking;
    
    public Noodles Noodles { get; private set; }
    
    protected override void OnTouch()
    {
        Debug.Log(GeneralSettings.Data.NoodleBoilingTime);        
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

    private void OnCookingStart()
    {
        //yield return new WaitForSeconds()
    }
}
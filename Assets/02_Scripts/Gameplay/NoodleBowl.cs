using System;

public class NoodleBowl : ItemRendererBase
{
    public void Start()
    {
        RenderItem(References.Instance.Items.Noodles);
    }

    protected override void OnTouch()
    {   
        NoodlePotDistributor.AddNoodles();
    }
}
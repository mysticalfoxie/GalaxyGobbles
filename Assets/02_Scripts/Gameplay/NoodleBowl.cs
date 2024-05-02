using System;

public class NoodleBowl : SpriteRendererBase
{
    public void Start()
    {
        RenderSprite(References.Instance.Items.Noodles);
    }

    protected override void OnTouch()
    {
        NoodlePotDistributor.AddNoodles();
    }
}
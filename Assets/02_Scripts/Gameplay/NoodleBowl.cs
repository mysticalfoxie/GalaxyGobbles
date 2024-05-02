using System;

public class NoodleBowl : TouchableMonoBehaviour
{
    public void Start()
    {
        //RenderSprite(References.Instance.Items.Noodles);
    }

    protected override void OnTouch()
    {
        NoodlePotDistributor.AddNoodles();
    }
}
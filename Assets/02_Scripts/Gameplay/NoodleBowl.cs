using System;

public class NoodleBowl : ItemDispenserBase
{
    public void Start()
    {
        SetItem(References.Instance.Items.Noodles);
    }

    protected override void OnTouch()
    {   
        NoodlePotDistributor.Instance.AddNoodles();
    }
}
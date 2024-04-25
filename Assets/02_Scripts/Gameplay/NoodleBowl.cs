public class NoodleBowl : ItemDispenserBase
{
    public override void Awake()
    {
        base.Awake();
        
        SetItem(References.Instance.Items.Noodles);
    }

    protected override void OnTouch()
    {   
        NoodlePotDistributor.Instance.AddNoodles();
    }
}
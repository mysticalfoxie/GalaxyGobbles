public class NoodlePot : TouchableMonoBehaviour
{
    private bool _cooking;
    
    public Noodles Noodles { get; private set; }
    
    protected override void OnTouch()
    {
        
    }

    public void SetNoodles(Noodles noodles)
    {
        if (Noodles is not null) return;
        
        Noodles = noodles;
    }
}
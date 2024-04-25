using System.Linq;

public class NoodlePotDistributor : SingletonMonoBehaviour<NoodlePotDistributor>
{
    public void AddNoodles()
    {
        var slot = References.Instance.NoodlePots.FirstOrDefault(x => x.Noodles is null);
        if (slot is null) return;

        var noodles = Noodles.Create();
        slot.SetNoodles(noodles);
    }
}
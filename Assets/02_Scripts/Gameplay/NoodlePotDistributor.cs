using System.Linq;

public class NoodlePotDistributor : SingletonMonoBehaviour<NoodlePotDistributor>
{
    public static void AddNoodles()
    {
        var slot = References.Instance.NoodlePots.FirstOrDefault(x => x.State == NoodlePotState.Empty);
        if (slot is null) return;

        slot.StartCooking();
    }
}
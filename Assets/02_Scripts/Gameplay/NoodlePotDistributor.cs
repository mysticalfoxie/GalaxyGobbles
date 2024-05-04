using System.Linq;
using UnityEngine;

public class NoodlePotDistributor : SingletonMonoBehaviour<NoodlePotDistributor>
{
    public static void AddNoodles()
    {
        var slot = References.Instance.NoodlePots.FirstOrDefault(x => x.Noodles is null);
        if (slot is null) return;

        var noodles = ScriptableObject.CreateInstance<NoodleData>();
        slot.SetNoodles(noodles);
    }
}
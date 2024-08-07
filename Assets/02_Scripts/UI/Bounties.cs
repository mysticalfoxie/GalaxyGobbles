using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Bounties : MonoBehaviour
{
    private readonly List<BountyData> _bounties = new();
    private GameObject[] _slots;
    private ItemData _token;

    public void Awake()
    {
        _slots = this.GetChildren().ToArray();
        _token = GameSettings.GetItemMatch(Identifiers.Value.BountyToken);
    }

    public bool TryAdd(BountyData bounty)
    {
        if (_bounties.Count >= _slots.Length) return false;
        
        var item = new Item(new(this, _token, true));
        var slot = _slots[_bounties.Count];
        item.Follow(slot);
        bounty.Token = item;
        _bounties.Add(bounty);
        
        return true;
    }

    public BountyData[] GetBounties()
    {
        return _bounties.ToArray();
    }

    public void Reset()
    {
        _bounties.Clear();
    }
}
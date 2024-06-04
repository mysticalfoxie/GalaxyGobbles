using UnityEngine;


public class BountyData : ScriptableObject
{
    public SpeciesData Species { get; set; }
    public bool WasTarget { get; set; }
    public Item Token { get; set; }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Customer", menuName = "Galaxy Gobbles/Customer", order = 2)]
public class CustomerData : ScriptableObject
{
    [Header("Customer Arrival")]
    [SerializeField] 
    [Range(0, 10)] 
    protected uint _minutesInGame;
    
    [SerializeField] 
    [Range(0, 59)]
    protected uint _secondsInGame;
    
    [Header("Customer Specification")]
    [SerializeField] private SpeciesData _species;
    [FormerlySerializedAs("_desiredItems")] [SerializeField] private string[] _desiredItemIds;

    [Header("Assassination")]
    [SerializeField] private bool _isAssassinationTarget;
    
    public uint Minutes => _minutesInGame;
    public uint Seconds => _secondsInGame;
    public SpeciesData Species => _species;
    public IEnumerable<string> DesiredItemIds => _desiredItemIds;
}
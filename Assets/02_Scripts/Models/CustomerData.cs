using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] private ItemId[] _desiredItems;

    [Header("Assassination")]
    [SerializeField] private bool _isAssassinationTarget;
    
    public uint Minutes => _minutesInGame;
    public uint Seconds => _secondsInGame;
    public SpeciesData Species => _species;
    public IEnumerable<ItemId> DesiredItems => _desiredItems;
}
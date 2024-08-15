using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Customer", menuName = "Galaxy Gobbles/Customer", order = 2)]
public class CustomerData : ScriptableObject
{
    [Header("Customer Arrival")]
    [SerializeField] 
    [Range(0, 10)] 
    internal uint _minutesInGame;
    
    [SerializeField] 
    [Range(0, 59)]
    internal uint _secondsInGame;
    
    [Header("Customer Specification")]
    [SerializeField] internal SpeciesData _species;
    [SerializeField] internal ItemData[] _desiredItems = Array.Empty<ItemData>();
    
    public int Minutes => (int)_minutesInGame;
    public int Seconds => (int)_secondsInGame;
    public SpeciesData Species => _species;
    public IEnumerable<ItemData> DesiredItems => _desiredItems;
}
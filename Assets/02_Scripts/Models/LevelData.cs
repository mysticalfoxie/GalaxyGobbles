using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Galaxy Gobbles/Level", order = 1)]
public class LevelData : ScriptableObject
{
    [SerializeField]
    internal int _levelNumber;
    
    [Header("Store Closure")]
    [Range(0, 10)] 
    [SerializeField]
    internal uint _closeAfterMinutes;
    
    [SerializeField]
    [Range(0, 59)]
    internal uint _closeAfterSeconds;

    [Header("Assassination")] 
    [Tooltip("Resolves to: \"You must kill the {TARGET_TEXT} that comes into the restaurant.\"")]
    [SerializeField]
    internal string _targetText;
    
    [Header("Customers")]
    [SerializeField]
    internal CustomerData[] _customers;
    
    internal int Number => _levelNumber;
    public uint CloseAfterMinutes => _closeAfterMinutes;
    public uint CloseAfterSeconds => _closeAfterSeconds;
    public IEnumerable<CustomerData> Customers => _customers;
    public string TargetText => _targetText;
}
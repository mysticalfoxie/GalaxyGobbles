using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Galaxy Gobbles/Level", order = 1)]
public class LevelData : ScriptableObject
{
    [SerializeField]
    private int _levelNumber;
    
    [Header("Store Closure")]
    [Range(0, 10)] 
    [SerializeField]
    private uint _closeAfterMinutes;
    
    [SerializeField]
    [Range(0, 59)]
    private uint _closeAfterSeconds;

    [Header("Assassination")] 
    [Tooltip("Resolves to: \"You must kill the {TARGET_TEXT} that comes into the restaurant.\"")]
    [SerializeField]
    private string _targetText;
    
    [Header("Customers")]
    [SerializeField]
    private CustomerData[] _customers;
    
    public int Number => _levelNumber;
    public uint CloseAfterMinutes => _closeAfterMinutes;
    public uint CloseAfterSeconds => _closeAfterSeconds;
    public IEnumerable<CustomerData> Customers => _customers;
    public string TargetText => _targetText;
}
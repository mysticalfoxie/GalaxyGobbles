using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "GameData/Level", order = 1)]
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
    
    [Header("Customers")]
    [SerializeField]
    private CustomerData[] _customers;
    
    public int Number => _levelNumber;
    public uint CloseAfterMinutes => _closeAfterMinutes;
    public uint CloseAfterSeconds => _closeAfterSeconds;
    public CustomerData[] Customers => _customers;
}
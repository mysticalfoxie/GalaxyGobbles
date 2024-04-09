using UnityEngine;

[CreateAssetMenu(fileName = "Customer", menuName = "GameData/Customer", order = 2)]
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
    [SerializeField]
    private SpeciesData _species;

    [Header("Customer Desires")]
    [SerializeField]
    private MealData[] _desiredMeals;

    public uint Minutes => _minutesInGame;
    public uint Seconds => _secondsInGame;
    public SpeciesData Species => _species;
    public MealData[] DesiredMeals => _desiredMeals;
}
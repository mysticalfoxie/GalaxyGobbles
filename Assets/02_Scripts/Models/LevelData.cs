using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Level", menuName = "Galaxy Gobbles/Level", order = 1)]
public class LevelData : ScriptableObject
{
    [SerializeField] private int _levelNumber;
    
    [Header("Store Closure")]
    [Range(0, 10)] [SerializeField] private uint _closeAfterMinutes;
    [Range(0, 59)] [SerializeField] private uint _closeAfterSeconds;

    [Header("Assassination")] 
    [Tooltip("The target's species. The name and type will be rendered in the assassination briefing.")]
    [SerializeField] private SpeciesData _targetSpecies;
    [Tooltip("The target's position in the customer list. Example: 3 => The \"3rd\" broccoloid that comes into the store.")]
    [SerializeField] private int _targetPosition;
    
    [Header("Assassination - Debug")]
    [Tooltip("Just a readonly value for better configuration. This is the amount of customers are in the level filtered by their species.")]
    [SerializeField] private int _amountOfSpeciesInLevel;
    // ReSharper disable once NotAccessedField.Local -> Used as readonly checkup field in inspector
    [Tooltip("This checks if the customer is existent in the store.")]
    [SerializeField] private bool _targetExists;


    [Header("Score")]
    [Tooltip("The score required for receiving star 1.")]
    [SerializeField] private float _requiredScoreStar1;
    [Tooltip("The score required for receiving star 2.")]
    [SerializeField] private float _requiredScoreStar2;
    [Tooltip("The score required for receiving star 3.")]
    [SerializeField] private float _requiredScoreStar3;
    
    [Header("Score Auto Calculations")]
    [Tooltip("The percentage requirement for star 1.")]
    [SerializeField] private float _star1Percentage;
    [Tooltip("The percentage requirement for star 2.")]
    [SerializeField] private float _star2Percentage;
    [Tooltip("The percentage requirement for star 3.")]
    [SerializeField] private float _star3Percentage;
    [Tooltip("The maximum possible score for this level.")]
    [SerializeField] private float _maxScore; 
    
    [Header("Customers")]
    [SerializeField]
    private CustomerData[] _customers;
    
    public int Number => _levelNumber;
    public uint CloseAfterMinutes => _closeAfterMinutes;
    public uint CloseAfterSeconds => _closeAfterSeconds;
    public IEnumerable<CustomerData> Customers => _customers;
    public SpeciesData Target => _targetSpecies;
    public int TargetPosition => _targetPosition;
    public float MinScore => _requiredScoreStar1;
    public float RequiredScoreStar1 => _requiredScoreStar1;
    public float RequiredScoreStar2 => _requiredScoreStar2;
    public float RequiredScoreStar3 => _requiredScoreStar3;
    public float Star1Percentage => _star1Percentage;
    public float Star2Percentage => _star2Percentage;
    public float Star3Percentage => _star3Percentage;
    public float MaxScore => _maxScore;

    public void SetCustomers(CustomerData[] newCustomers)
    {
        _customers = newCustomers;
    }

    public void OnValidate()
    {
        ValidateTarget();
        ValidateScore();
    }

    public int GetCustomerPosition(CustomerData customer)
    {
        var index = 0;
        foreach (var entry in _customers.Where(x => x.Species.name == customer.Species.name))
        {
            index++;
            if (entry.name == customer.name)
                return index;
        }

        return -1;
    }

    private void ValidateTarget()
    {
        _amountOfSpeciesInLevel = _customers.Count(x => x.Species.name == _targetSpecies?.name);
        _targetExists = _targetPosition <= _amountOfSpeciesInLevel && _targetPosition != 0;
    }

    private void ValidateScore()
    {
        if (!GameSettings.Data) return;
        var allMealsScore = _customers.SelectMany(x => x.DesiredItems).Where(x => x is not null).Select(x => x.Score).Sum();
        var allBaseScores = _customers.Length * GameSettings.Data.CustomerBaseScore;
        var allMaxScores = _customers.Length * GameSettings.Data.CustomerMaxScore;
        _maxScore = allMaxScores + allBaseScores + allMealsScore;

        if (_targetExists)
        {
            var target = _customers.Where(x => x.Species.name == _targetSpecies.name).ElementAt(_targetPosition - 1);
            _maxScore += GameSettings.Data.SuccessFullAssassinationScore;
            _maxScore -= target.DesiredItems.Select(x => x.Score).Sum();
            _maxScore -= GameSettings.Data.CustomerMaxScore;
            _maxScore -= GameSettings.Data.CustomerBaseScore;
        }
        
        if (_maxScore == 0) return;
        
        _star1Percentage = 100.0F / _maxScore * _requiredScoreStar1;
        _star2Percentage = 100.0F / _maxScore * _requiredScoreStar2;
        _star3Percentage = 100.0F / _maxScore * _requiredScoreStar3;
    }
}
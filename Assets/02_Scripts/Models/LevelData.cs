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
    [Tooltip("Resolves to: \"You must kill the {TARGET_TEXT} that comes into the restaurant.\"")]
    [SerializeField] private string _targetText;

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
    public string TargetText => _targetText;
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
        var allMealsScore = _customers.SelectMany(x => x.DesiredItems).Where(x => x is not null).Select(x => x.Score).Sum();
        var allBaseScores = _customers.Length * GameSettings.Data.CustomerBaseScore;
        var allMaxScores = _customers.Length * GameSettings.Data.CustomerMaxScore;
        _maxScore = allMaxScores + allBaseScores + allMealsScore;
        if (_maxScore == 0) return;
        
        _star1Percentage = 100.0F / _maxScore * _requiredScoreStar1;
        _star2Percentage = 100.0F / _maxScore * _requiredScoreStar2;
        _star3Percentage = 100.0F / _maxScore * _requiredScoreStar3;
    }
}
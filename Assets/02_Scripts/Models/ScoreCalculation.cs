using System.Collections.Generic;
using System.Linq;

public class ScoreCalculation
{
    public float Patience { get; }
    public IEnumerable<ItemData> Meals { get; }
    public float Score { get; private set; }

    public ScoreCalculation(float patience, IEnumerable<ItemData> meals)
    {
        Patience = patience;
        Meals = meals;
        Calculate();
    }

    private void Calculate()
    {
        var meals = Meals.Sum(x => x.Score);
        var patience = Patience * 0.01F; // 0-100 -> 0-1
        var patienceScore = patience * GameSettings.Data.CustomerMaxScore;
        var baseScore = meals + GameSettings.Data.CustomerBaseScore;
        Score = patienceScore + baseScore;
    }

    public void Apply()
    {
        BottomBar.Instance.Score.Add(Score);
    }
}
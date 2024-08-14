using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;

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
        var patienceScore = GetPatienceScore(Patience);
        var baseScore = meals + GameSettings.Data.CustomerBaseScore;
        Score = patienceScore + baseScore;
    }

    public void Apply()
    {
        BottomBar.Instance.Score.Add(Score);
    }

    private static float GetPatienceScore(float patience)
    {
        if (patience > GameSettings.Data.CustomerLoveThreshold)
            return GameSettings.Data.CustomerMaxScore;
        if (patience < GameSettings.Data.CustomerAngryThreshold)
            return 0.0F;
        return patience * GameSettings.Data.CustomerMaxScore / 100;
    }
}

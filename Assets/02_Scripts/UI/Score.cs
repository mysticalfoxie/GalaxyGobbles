using System;
using System.Globalization;
using UnityEngine;

public class Score : MonoBehaviour
{
    [Header("Rounding")] 
    [SerializeField] [Range(0.0F, 1.0F)] private float _roundTo = 0.25F;
    [SerializeField] [Range(0, 6)] private int _decimals = 2;
    
    public float Value { get; private set; }

    public void Add(float points)
    {
        Value += Round(points);
        UpdateProgress();
    }

    public void Remove(float points)
    {
        Value -= points;
        UpdateProgress();
    }

    private void UpdateProgress()
    {
        var max = LevelManager.CurrentLevel.MaxScore;
        var value = Math.Min(Value, max);
        var percentage = 100.0F / max * value;
        var scoreValue = BottomBar.Instance.Score.Value;
        BottomBar.Instance.ProgressBar.SetValue(percentage);
        ProgressBar.Instance.SetScore(scoreValue);
    }

    private float Round(float score)
    {
        var parts = score.ToString(CultureInfo.InvariantCulture).Split('.');
        if (parts.Length <= 1) return score;
        var decimalString = parts[1].Length > _decimals ? parts[1][.._decimals] : parts[1].PadRight(_decimals, '0');
        var decimals = int.Parse(decimalString);
        var factor = Mathf.Floor(decimals / 25.0F);
        var ceil = decimals % (_roundTo * 100) > _roundTo * 100 / 2 ? 1.0F : 0.0F;
        var rounded = int.Parse(parts[0]) + _roundTo * (factor + ceil);
		
        return rounded;
    }

    public void Reset()
    {
        Value = 0.0F;
    }
}
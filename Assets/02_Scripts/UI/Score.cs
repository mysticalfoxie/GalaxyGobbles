using System;
using UnityEngine;

public class Score : MonoBehaviour
{
    public float Value { get; private set; }

    public void Add(float points)
    {
        Value += points;
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

    public void Reset()
    {
        Value = 0.0F;
    }
}
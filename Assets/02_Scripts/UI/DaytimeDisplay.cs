using System;
using UnityEngine;
using UnityEngine.UI;

public class DaytimeDisplay : MonoBehaviour
{
    private Text _renderer;

    public void Awake()
    {
        _renderer = this.GetRequiredComponent<Text>();
    }

    public void UpdateTime(int totalSeconds)
    {
        var minutes = Math.Abs(totalSeconds / 60);
        var seconds = Math.Abs(totalSeconds % 60);
        var mm = minutes.ToString().PadLeft(2, '0');
        var ss = seconds.ToString().PadLeft(2, '0');
        var minus = totalSeconds < 0 ? '-' : ' ';
        _renderer.text = $"{minus}{mm}:{ss}";
    }
}
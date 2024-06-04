using System;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class Progressbar : MonoBehaviour
{
    public static int Progress;

    public static Progressbar Instance { get; set; }

    [SerializeField] private Slider _progressSlider;
    [SerializeField] GameObject _allGoalClearedCheckmark;
    [SerializeField] TMP_Text _levelIndicator;
    [SerializeField] [Range(0, 100)] private int _starOneReached;
    [SerializeField] [Range(0, 100)] private int _starTwoReached;
    [SerializeField] [Range(0, 100)] private int _starThreeReached;

    private void Awake()
    {
        if (!_progressSlider) _progressSlider = gameObject.GetComponentInChildren<Slider>();
        var level = (LevelManager.CurrentLevelIndex + 1).ToString().PadLeft(2, '0');
        _levelIndicator.text = $"Level #{level}";
    }

    public void ProgressSlider(int sliderValue)
    {
        _progressSlider.value++;
        CheckSliderValue();
    }

    private void CheckSliderValue()
    {
        if (_progressSlider.value >= _starOneReached && _progressSlider.value < _starTwoReached)
        {
            Progress = 1;
        }
        else if (_progressSlider.value >= _starTwoReached && _progressSlider.value < _starThreeReached)
        {
            Progress = 2;
        }
        else if (_progressSlider.value >= _starThreeReached)
        {
            Progress = 3;
        }

        if (_progressSlider.value > 99)
        {
            _allGoalClearedCheckmark.SetActive(true);
        }
    }
    public void OnClickFillProgressbar()
    {
        _progressSlider.value++;
        Debug.Log($"Set Progressbar to {_progressSlider.value}");
    }

    public void Reset()
    {
        _progressSlider.value = 0;
        _allGoalClearedCheckmark.SetActive(false);
        Progress = 0;
    }
}
using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : Singleton<ProgressBar>
{
    public static int Progress;
    
    private float _starOneReached;
    private float _starTwoReached;
    private float _starThreeReached;
    private Slider _progressSlider;
    
    [SerializeField] private RectTransform _starOneReachedMark;
    [SerializeField] private RectTransform _starTwoReachedMark;
    [SerializeField] private RectTransform _starThreeReachedMark;
    [SerializeField] private TMP_Text _scoreIndicator;
    [SerializeField] private float _scoreMin;
    private RectTransform _rectTransform;


    public override void Awake()
    {
        InheritedDDoL = true;
        base.Awake();
        _progressSlider = this.GetRequiredComponent<Slider>();
        _rectTransform = _progressSlider.GetRequiredComponent<RectTransform>();
    }

    public void SetValue(float sliderValue)
    {
        _progressSlider.value = sliderValue;
        CheckSliderValue();
    }

    public void SetScore(float scoreValue)
    {
        _scoreIndicator.text = scoreValue.ToString(CultureInfo.InvariantCulture);
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
    }

    public void OnLevelLoaded()
    { 
        _starOneReached = ClampSliderPosition(LevelManager.CurrentLevel.Star1Percentage, 1);
        _starTwoReached = ClampSliderPosition(LevelManager.CurrentLevel.Star2Percentage, 2);
        _starThreeReached = ClampSliderPosition(LevelManager.CurrentLevel.Star3Percentage, 3);
        
        _progressSlider.value = 0;
        Progress = 0;
        _scoreIndicator.text = "0";
        
        var max = _rectTransform.rect.width;
        var starOneReachedX = max * 0.01F * _starOneReached;
        var starTwoReachedX = max * 0.01F * _starTwoReached;
        var starThreeReachedX = max * 0.01F * _starThreeReached;
        _starOneReachedMark.anchoredPosition = new Vector2(starOneReachedX, _starOneReachedMark.anchoredPosition.y);
        _starTwoReachedMark.anchoredPosition = new Vector2(starTwoReachedX, _starTwoReachedMark.anchoredPosition.y);
        _starThreeReachedMark.anchoredPosition = new Vector2(starThreeReachedX, _starThreeReachedMark.anchoredPosition.y);
    }

    private float ClampSliderPosition(float value, int startIndex)
    {
        if (value > 100) Debug.LogError($"[Progress Bar] The score for slider indicator #0{startIndex} is above 100%! Please adjust the score for this level.");
        if (value < _scoreMin) Debug.LogError($"[Progress Bar] The score for slider indicator #0{startIndex} is below the minimum of {_scoreMin}%! Please adjust the score for this level to avoid overlapping.");
        return Mathf.Max(Mathf.Min(value, 100), _scoreMin);
    }
}

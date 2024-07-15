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


    public override void Awake()
    {
        InheritedDDoL = true;
        base.Awake();
        _progressSlider = this.GetRequiredComponent<Slider>();
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
        _starOneReached = LevelManager.CurrentLevel.Star1Percentage;
        _starTwoReached = LevelManager.CurrentLevel.Star2Percentage;
        _starThreeReached = LevelManager.CurrentLevel.Star3Percentage;
        _progressSlider.value = 0;
        Progress = 0;
        _scoreIndicator.text = 0.ToString(CultureInfo.InvariantCulture);
 
        var rectTransform = _progressSlider.GetRequiredComponent<RectTransform>();
        var max = rectTransform.rect.width;
        var starOneReachedX = max * 0.01F * _starOneReached;
        var starTwoReached = max * 0.01F * _starTwoReached;
        var starThreeReached = max * 0.01F * _starThreeReached;
        _starOneReachedMark.anchoredPosition = new Vector2(starOneReachedX, _starOneReachedMark.anchoredPosition.y);
        _starTwoReachedMark.anchoredPosition = new Vector2(starTwoReached, _starTwoReachedMark.anchoredPosition.y);
        _starThreeReachedMark.anchoredPosition = new Vector2(starThreeReached, _starThreeReachedMark.anchoredPosition.y);
    }
}
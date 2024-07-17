using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] GameObject[] _starsObjects;
    [SerializeField] private Sprite _goldenStarSprite;
    [SerializeField] private GameObject _levelButton;

    public static int UnlockedLevels;
    public int LevelIndex { get; set; }
    public static object Instance { get; set; }
    private float _canvasScaling = CanvasScaling.ScaleFactor;

    public event Action<int> Clicked;

    public void Awake()
    {
    }

    public void Update()
    {
        var buttonScale = _levelButton.transform.localScale;
        if (buttonScale != Vector3.one * (_canvasScaling * 0.75f))
        {
            RectTransform parentRect = (_levelButton.transform as RectTransform);
            var xScalingFactor = _canvasScaling * 0.75f;
            var yScalingFactor = _canvasScaling * 0.75f;
            if (parentRect != null) parentRect.localScale = new Vector3(xScalingFactor, yScalingFactor, 0);
        }
    }

    public void AddStars()
    {
        var unlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 0);
        if (unlockedLevels < LevelIndex) return;
        var button = GetComponent<Button>();
        button.interactable = true;
        UpdateStars();
    }

    public void UpdateStars()
    {
        var stars = PlayerPrefs.GetInt("Stars" + LevelIndex.ToString(), 0);
        for (var j = 0; j < stars; j++)
        {
            var starImage = _starsObjects[j].GetComponent<Image>();
            starImage.sprite = _goldenStarSprite;
        }
    }

    public void OnClick()
    {
        Clicked?.Invoke(LevelIndex);
    }
}
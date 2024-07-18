using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] private GameObject[] _starsObjects;
    [SerializeField] private Sprite _goldenStarSprite;
    [SerializeField] private GameObject _levelButton;

    public static int UnlockedLevels;
    public int LevelIndex { get; set; }

    public event Action<int> Clicked;
    
    public void Update()
    {
        // var buttonScale = _levelButton.transform.localScale;
        // if (buttonScale != Vector3.one * (CanvasScaling.ScaleFactor * GameSettings.Data.LevelButtonScale))
        // {
        //     var parentRect = _levelButton.transform as RectTransform;
        //     var xScalingFactor = CanvasScaling.ScaleFactor * GameSettings.Data.LevelButtonScale;
        //     var yScalingFactor = CanvasScaling.ScaleFactor * GameSettings.Data.LevelButtonScale;
        //     if (parentRect) parentRect!.localScale = new Vector3(xScalingFactor, yScalingFactor, 0);
        // }
        
        RefreshStars();
    }

    public void RefreshStars()
    {
        AddStars();
    }

    public void AddStars()
    {
        var unlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 0);
        if (unlockedLevels < LevelIndex) return;
        var button = this.GetRequiredComponent<Button>();
        button.interactable = true;
        UpdateStars();
    }

    public void UpdateStars()
    {
        var stars = PlayerPrefs.GetInt("Stars" + LevelIndex.ToString(), 0);
        for (var i = 0; i < stars; i++)
        {
            var starImage = _starsObjects[i].GetRequiredComponent<Image>();
            starImage.sprite = _goldenStarSprite;
        }
    }

    public void OnClick()
    {
        Clicked?.Invoke(LevelIndex);
    }
}
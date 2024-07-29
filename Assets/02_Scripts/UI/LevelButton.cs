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

    public void OnEnable()
    {
        var xScalingFactor = GameSettings.Data.LevelButtonScale;
        var yScalingFactor = GameSettings.Data.LevelButtonScale;
        transform.localScale *= new Vector2(xScalingFactor, yScalingFactor);

        RefreshStars();
    }

    public void RefreshStars()
    {
        AddStars();
    }

    public void AddStars()
    {
        var unlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 0);
        // var unlockedLevels = PlayerPrefs.HasKey("UnlockedLevels")
        //     ? PlayerPrefs.GetInt("UnlockedLevels", 0)
        //     : PlayerPrefs.GetInt("UnlockedLevels");
        UpdateStars();
        if (unlockedLevels < LevelIndex) return;
        var button = this.GetRequiredComponent<Button>();
        button.interactable = true;
    }

    public void UpdateStars()
    {
        var stars = PlayerPrefs.GetInt("Stars" + LevelIndex, 0);
        // var stars = PlayerPrefs.HasKey("Stars")
        //     ? PlayerPrefs.GetInt("Stars" + LevelIndex, 0)
        //     : PlayerPrefs.GetInt("Stars");
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
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
    public UnityEngine.UI.Button LevelButtonski;
    public event Action<int> Clicked;

    public void OnEnable()
    {
        LevelButtonski = this.GetRequiredComponent<Button>();
        var unlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 0);
        UpdateStars(unlockedLevels);
        UpdateLevel(unlockedLevels);
    }

    public void AddStars()
    {
        var unlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 0);
        LevelButtonski.interactable = LevelIndex <= unlockedLevels;
        UpdateStars(unlockedLevels);
    }

    public void UpdateLevel(int unlockedLevels)
    {
        LevelButtonski.interactable = LevelIndex <= unlockedLevels;
    }

    public void UpdateStars(int unlockedLevels)
    {
        var stars = PlayerPrefs.GetInt("Stars" + LevelIndex, 0);
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
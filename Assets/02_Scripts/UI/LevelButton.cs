using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] GameObject[] _starsObjects;
    [SerializeField] private Sprite _goldenStarSprite;
    
    public static int UnlockedLevels;
    public int LevelIndex { get; set; }
    public static object Instance { get; set; }

    public event Action<int> Clicked;

    public void AddStars()
    {
        var unlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 0);
        if (unlockedLevels < LevelIndex) return;
        var button = GetComponent<Button>();
        button.interactable = true;
        UpdateStars	();
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
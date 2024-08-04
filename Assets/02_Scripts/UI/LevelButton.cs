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

    public void Awake()
    {
        var levelButtonScale = (Vector2)transform.localScale;
        var scalingFactor = new Vector2(GameSettings.Data.LevelButtonScale, GameSettings.Data.LevelButtonScale);
        if (levelButtonScale != scalingFactor)
            transform.localScale *= scalingFactor;
        LevelButtonski = this.GetRequiredComponent<Button>();
    }

    public void OnEnable()
    {
        UpdateStars();
        UpdateLevel();
    }

    public void AddStars()
    {
        var unlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 0);
        LevelButtonski.interactable = LevelIndex <= unlockedLevels;
        UpdateStars();
    }

    public void UpdateLevel()
    {
        var unlockedLevels = PlayerPrefs.GetInt("UnlockedLevels", 0);
        LevelButtonski.interactable = unlockedLevels >= LevelIndex;
    }

    public void UpdateStars()
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
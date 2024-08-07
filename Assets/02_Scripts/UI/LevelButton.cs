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
    }

    public void Refresh()
    {
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
        // Todo: Fix me - I'm hard stuck at level 3
        // It seems like the lines where "UnlockedLevels" is set are corrupted, not this here. 
        LevelButtonski.interactable = true; // LevelIndex <= unlockedLevels;
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
        AudioManager.Instance.PlaySFX(AudioSettings.Data.UIOpenPopup);
        Clicked?.Invoke(LevelIndex);
    }
}
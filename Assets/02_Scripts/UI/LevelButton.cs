using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [Header("Stars")]
    [SerializeField] private Image[] _stars;
    [SerializeField] private Sprite _filled;
    [SerializeField] private Sprite _unfilled;

    public event Action Clicked;
    public int LevelNumber { get; private set; }

    private Button _button;
    private TMP_Text _label;

    public void OnEnable()
    {
        _button = this.GetRequiredComponent<Button>();
        _label = this.GetRequiredComponentInChildren<TMP_Text>();
    }

    private void SetStars(int stars)
    {
        for (var i = 0; i < _stars.Length; i++)
            _stars[i].sprite = i <= stars - 1 ? _filled : _unfilled;
    }

    public void OnClick()
    {
        AudioManager.Instance.PlaySFX(AudioSettings.Data.UIOpenPopup);
        Clicked?.Invoke();
    }

    public void SetData(LevelSaveData data)
    {
        LevelNumber = data.Number;
        _button.interactable = data.Unlocked;
        _label.text = $"Level {data.Number.ToString().PadLeft(2, '0')}";
        SetStars(data.Stars);
    }
}
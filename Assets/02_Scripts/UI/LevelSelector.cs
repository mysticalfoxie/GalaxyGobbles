using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private GameObject _levelButtonPrefab;
    [SerializeField] private GameObject _parentLevelButton;

    private bool _levelLoading;

    private void Awake()
    {
        ButtonFunction();
    }

    public void ButtonFunction()
    {
        var levels = GameSettings.Data.Levels.ToArray();
        for (var i = 0; i < levels.Length; i++)
        {
            var levelButton = Instantiate(_levelButtonPrefab);
            levelButton.transform!.SetParent(_parentLevelButton.transform);
            var levelNumber = levelButton.GetComponentInChildren<TMP_Text>();
            levelNumber.text = "Level "+(i + 1).ToString();
            var buttonScript = levelButton.GetRequiredComponent<LevelButton>();
            levelButton.GetRequiredComponent<Button>().interactable = true; // unlock all levels for Gate I build. ToDo:[! Delete for Goldmaster !]
            buttonScript.LevelIndex = i;
            buttonScript.AddStars();
            
            var ci = i;
            buttonScript.Clicked += _ => MainMenu.Instance.StartLoadingLevel(ci);
        }
    }
}
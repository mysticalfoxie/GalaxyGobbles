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
        var levels = GameSettings.Data.Levels.ToArray();
        for (var i = 0; i < levels.Length; i++)
        {
            var levelButton = Instantiate(_levelButtonPrefab);
            levelButton.transform!.SetParent(_parentLevelButton.transform);
            var levelNumber = levelButton.GetComponentInChildren<TMP_Text>();
            levelNumber.text = (i + 1).ToString();
            var buttonScript = levelButton.GetRequiredComponent<LevelButton>();
            levelButton.GetRequiredComponent<Button>().interactable = true; // unlock all levels for Gate I build.
            buttonScript.LevelIndex = i;
            buttonScript.AddStars();
            
            var ci = i;
            buttonScript.Clicked += _ => OnLevelButtonClicked(ci);
        }
    }

    private void OnLevelButtonClicked(int index)
    {
        // The level is already loading -> Do nothing -> Return
        if (_levelLoading) return;
        
        // Starting to load the level
        _levelLoading = true;
        
        StartCoroutine(LoadLevelAsync(index));
    }

    private IEnumerator LoadLevelAsync(int index)
    {
        // TODO: Fade black before the scene loads
        
        // Before the new level has started loading  
        yield return LevelManager.Instance.LoadLevelAsync(index);
        // After the level has completely loaded
        
        // TODO: Fade back to the game when the scene has completely loaded! :)
        // TODO: Show Bounty Screen
        
        // When done with level loading the button becomes clickable again
        // -> Stay at the last line of the function
        _levelLoading = false;
    }
}
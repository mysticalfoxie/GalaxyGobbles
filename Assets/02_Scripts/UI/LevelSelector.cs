using System.Linq;
using TMPro;
using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private GameObject _levelButtonPrefab;
    [SerializeField] private GameObject _parentLevelButton;

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
            buttonScript.LevelIndex = i;
            buttonScript.Clicked += index => LevelManager.Instance.LoadLevel(index);
            buttonScript.AddStars();
        }
    }

}
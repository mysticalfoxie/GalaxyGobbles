using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public static int UnlockedLevels;
    [SerializeField] private GameObject _levelButton;
    [SerializeField] private Sprite _goldenStarSprite;

    [SerializeField] private GameObject _parentLvlBtn;
    private List<GameObject> _buttons = new List<GameObject>();

    void Awake()
    {
        var levels = GameSettings.Data.Levels.ToArray();
        for (int i = 0; i < levels.Length; i++)
        {
            var lvlBtn = Instantiate(_levelButton);
            lvlBtn.transform!.SetParent(_parentLvlBtn.transform);
            var lvlNum = lvlBtn.GetComponentInChildren<TMP_Text>();
            lvlNum.text = (i + 1).ToString();
            
            var buttonScript = lvlBtn.GetRequiredComponent<LevelButton>();
            buttonScript.LevelIndex = i;
            buttonScript.Clicked += LevelManager.Instance.LoadLevel; 
            
            _buttons.Add(lvlBtn);
        }
    }
    void Start()
    {
        UnlockedLevels = PlayerPrefs.GetInt("UnlockedLevels",0);
        for (int i = 0; i < _buttons.Count; i++)
        {
            if (UnlockedLevels < i) continue;
            var button = _buttons[i].GetComponent<Button>();
            button.interactable = true;
            var stars = PlayerPrefs.GetInt("Stars" + i.ToString(), 0);
            for (int j = 0; j < stars; j++)
            {
                var buttonStars = _levelButton.GetComponentsInChildren<Image>(); 
                buttonStars[j].sprite = _goldenStarSprite;
            }

        }
    }
}

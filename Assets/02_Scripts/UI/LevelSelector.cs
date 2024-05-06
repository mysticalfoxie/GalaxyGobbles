using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelector : MonoBehaviour
{
    public LevelObjects[] LevelObjects;
    public static int UnlockedLevels;
    [SerializeField] private GameObject _levelButton;
    [SerializeField] Sprite GoldenStarSprite;

    [SerializeField] private GameObject _parentLvlBtn;
    private List<GameObject> _buttons = new List<GameObject>();
    
    public static int CurrentLevel;

    public int availableLevels;

    public int SelectedLevel;
    //[SerializeField] TMP_Text _levelText;

    void Awake()
    {
        for (int i = 0; i < LevelSettings.Data.Levels.Length; i++)
        {
            var lvlBtn = Instantiate(_levelButton);
            lvlBtn.transform!.SetParent(_parentLvlBtn.transform);
            var lvlNum = lvlBtn.GetComponentInChildren<TMP_Text>();
            lvlNum.text = (i + 1).ToString();
            _buttons.Add(lvlBtn);
        }
    }
    void Start()
    {
        UnlockedLevels = PlayerPrefs.GetInt("UnlockedLevels",0);
        for (int i = 0; i < _buttons.Count; i++)
        {
            if (UnlockedLevels >= i)
            {
                var button = _buttons[i].GetComponent<Button>();
                button.interactable = true;
                int stars = PlayerPrefs.GetInt("stars " + i.ToString(), 0);
                for (int j = 0; j < stars; j++)
                {
                    var buttonStar = _levelButton.GetComponentInChildren<Sprite>(); 
                    buttonStar.sprite = GoldenStarSprite;
                }
            }
            
        }
    }
    
    public void OpenScene(int levelNum)
    {
        SelectedLevel = levelNum;
        MainMenu.Instance.SetElementsForStart();
        SceneManager.LoadSceneAsync("0.1_Level");
    }
}

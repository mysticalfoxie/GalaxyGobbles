using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
//  [Header("Options")]
//  [Header("Dropdown")]
    [Header("Menus")] 
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _startMenu;
    [SerializeField] private GameObject _completeDayMenu;
    [SerializeField] private GameObject _sidebar;
    [SerializeField] private GameObject _levelMap;
    [SerializeField] private GameObject _options;
    [SerializeField] private GameObject _credits;
    
    [Header("Button")]
    [SerializeField] private GameObject _btnMainMenu;
    
    [Header("Slider")]
    
    [SerializeField] private GameObject _backgroundImage;
    [SerializeField] private bool _startWithoutMenu;
    
    private bool _pausedGame;
    private bool _blockPauseMenu;
    public static MainMenu Instance { get; private set; } 

    public void Start()
    {
        _startMenu.SetActive(!_startWithoutMenu);
      //  _backgroundImage.SetActive(true);
        
        /*
         * WIP: Used for Audio Control, later content!
         * _backgroundAudio = FindObjectOfType<AudioSource>();
         * if(_backgroundAudio) _backgroundAudio.Play();
         */
    }

    public void Awake()
    {
        if (Instance is not null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public void StartGame()
    {
        //SceneManager.LoadScene(1);
        _startMenu.SetActive(false);
        _levelMap.SetActive(true);
    }

    public void SetElementsForStart()
    {
        _levelMap.SetActive(false);
        _blockPauseMenu = false;
        _btnMainMenu.SetActive(true);
        _sidebar.SetActive(true);    
    }

    public void PauseGame()
    {
        _btnMainMenu.SetActive(false);
        Time.timeScale = 0.0f;
        _pauseMenu.SetActive(true);
        _pausedGame = true;
    }

    public void ResumeGame()
    {
        _btnMainMenu.SetActive(true);
        Time.timeScale = 1.0f;
        _pauseMenu.SetActive(false);
        _pausedGame = false;
    }

    public void HomeMenu()
    {
        if (_completeDayMenu) _completeDayMenu.SetActive(false);
        if (Time.timeScale != 1.0f) Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
        if (_pauseMenu) _pauseMenu.SetActive(false);
        _startMenu.SetActive(true);
        _blockPauseMenu = true;
        _sidebar.SetActive(false);
    }

    public void BackButton()
    {
        _levelMap.SetActive(false);
        _startMenu.SetActive(true);
    }
    public void BackToLevelSelection()
    {
        SceneManager.LoadScene("0.0_StartScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void CompleteDay()
    {
        _btnMainMenu.SetActive(false);
        _completeDayMenu.SetActive(true);
    }
}

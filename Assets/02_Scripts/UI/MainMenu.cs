using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
//  [Header("Options")]
//  [Header("Dropdown")]
    [Header("Menus")] 
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _startMenu;
    [SerializeField] private GameObject _sidebar;
    
    [Header("Button")]
    [SerializeField] private GameObject _btn_MainMenu;
    
    [Header("Misc")]
    [SerializeField] private GameObject _backgroundImage;
    
    private bool _pausedGame;
    private bool _blockPauseMenu;

    public void Start()
    {
        _startMenu.SetActive(true);
      //  _backgroundImage.SetActive(true);
        
        /*
         * WIP: Used for Audio Control, later content!
         * _backgroundAudio = FindObjectOfType<AudioSource>();
         * if(_backgroundAudio) _backgroundAudio.Play();
         */
    }

    public void Update()
    {
       // if (_blockPauseMenu) return;
        /* if (!_pausedGame)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
        */
        
    }

    public void StartGame()
    {
        _blockPauseMenu = false;
        SceneManager.LoadScene(1);
        _btn_MainMenu.SetActive(true);
        _startMenu.SetActive(false);
        _sidebar.SetActive(true);
    }

    public void PauseGame()
    {
        _btn_MainMenu.SetActive(false);
        Time.timeScale = 0.0f;
        _pauseMenu.SetActive(true);
        _pausedGame = true;
    }

    public void ResumeGame()
    {
        _btn_MainMenu.SetActive(true);
        Time.timeScale = 1.0f;
        _pauseMenu.SetActive(false);
        _pausedGame = false;
    }

    public void HomeMenu()
    {
        if (Time.timeScale != 1.0f) Time.timeScale = 1.0f;
        SceneManager.LoadScene(0);
        if (_pauseMenu) _pauseMenu.SetActive(false);
        _startMenu.SetActive(true);
        _blockPauseMenu = true;
        _sidebar.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    

}

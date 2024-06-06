using System.Collections;
using Unity.VisualScripting;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public const int MAIN_MENU_SCENE_INDEX = 0;
    
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

    [Header("Audio")]
    [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private float _currentVolume;
    [SerializeField] private float _currentMusicVolume;
    [SerializeField] private float _currentSfxVolume;
    
    [SerializeField] private GameObject _backgroundImage;
    [SerializeField] private bool _startWithoutMenu;

    [SerializeField] TMP_Text _completeDayText;
    
    private bool _pausedGame;
    private bool _blockPauseMenu;
    private bool _levelLoading;
    public static MainMenu Instance { get; private set; }

    public void Awake()
    {
        if (Instance is not null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }
    public void Start()
    {
        _startMenu.SetActive(!_startWithoutMenu);
                
        LoadSettings();
      //  _backgroundImage.SetActive(true);
        
        /*
         * WIP: Used for Audio Control, later content!
         * _backgroundAudio = FindObjectOfType<AudioSource>();
         * if(_backgroundAudio) _backgroundAudio.Play();
         */
    }

    public void StartGame()
    {
        //SceneManager.LoadScene(1);
        _startMenu.SetActive(false);
        _levelMap.SetActive(true);
    }

    public void SetElementsForStart()
    {
        if (Time.timeScale != 1.0f) Time.timeScale = 1.0f;
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
        if (_pauseMenu) _pauseMenu.SetActive(false);
        _startMenu.SetActive(true);
        _blockPauseMenu = true;
        _sidebar.SetActive(false);
        SceneManager.LoadScene(MAIN_MENU_SCENE_INDEX);
    }

    public void Options()
    {
        _options.SetActive(true);
    }

    public void SetVolume(float masterVolume)
    {
        _currentVolume = masterVolume;
        _audioMixer.SetFloat("MasterVolume", masterVolume);
    }
    public void SetMusic(float musicVolume)
    {
        _currentMusicVolume = musicVolume;
        _audioMixer.SetFloat("MusicVolume", musicVolume);
    }
    public void SetSfx(float sfxVolume)
    {
        _currentSfxVolume = sfxVolume;
        _audioMixer.SetFloat("SFXVolume", sfxVolume);
    }

    public void BackButton()
    {
        if(_options) _options.SetActive(false);
        _levelMap.SetActive(false);
        _startMenu.SetActive(true);
    }
    public void BackToLevelSelection()
    {
        if (_completeDayText != null) _completeDayText.text = null;
        if (_startMenu) _startMenu.SetActive(false);
        if (_completeDayMenu) _completeDayMenu.SetActive(false);
        if (Time.timeScale != 1.0f) Time.timeScale = 1.0f;
        if (_pauseMenu) _pauseMenu.SetActive(false);
        if (_blockPauseMenu == false) _blockPauseMenu = true;
        if (_sidebar)_sidebar.SetActive(false);
        _levelMap.SetActive(true);
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
        
#if UNITY_EDITOR // UwU
        EditorApplication.ExitPlaymode();
#endif
    }

    public void CompleteDay()
    {
        var starsAcquired = ProgressBar.Progress;
        _btnMainMenu.SetActive(false);
        _completeDayMenu.SetActive(true);
        
        if (starsAcquired > PlayerPrefs.GetInt("Stars" + LevelManager.CurrentLevelIndex)) 
            PlayerPrefs.SetInt("Stars" + LevelManager.CurrentLevelIndex, starsAcquired);
        
        if (starsAcquired >= 1)
        {
            if(LevelButton.UnlockedLevels == LevelManager.CurrentLevelIndex)
                LevelButton.UnlockedLevels++;
            
            PlayerPrefs.SetInt("UnlockedLevels", LevelButton.UnlockedLevels);
            
            //_completeDayText.text = $"You Completed Day {LevelManager.CurrentLevelIndex + 1} and Acquired {starsAcquired} Stars!";
        }
        else
        {
            // Temporary for Gate I (Always succeed + pass to next level)
            //_completeDayText.text = "You completed day #" + (LevelManager.CurrentLevelIndex + 1).ToString().PadLeft(2, '0');
            if(LevelButton.UnlockedLevels == LevelManager.CurrentLevelIndex) LevelButton.UnlockedLevels++;
            PlayerPrefs.SetInt("UnlockedLevels", LevelButton.UnlockedLevels);
            
            // Original: _completeDayText.text = "You didn't Completed Day " + (LevelManager.CurrentLevelIndex + 1);
        }
    }

    public void BackAndSave()
    {
        Save();
        if (_pausedGame) _options.SetActive(false);
        else BackButton();
    }
    public void Save()
    {
        PlayerPrefs.SetFloat("MasterVolume", _currentVolume);
        PlayerPrefs.SetFloat("MusicVolume", _currentMusicVolume);
        PlayerPrefs.SetFloat("SFXVolume", _currentSfxVolume);
    }

    public void LoadSettings()
    {
        _volumeSlider.value = PlayerPrefs.HasKey("MasterVolume")
            ? _currentVolume = PlayerPrefs.GetFloat("MasterVolume")
            : PlayerPrefs.GetFloat("MasterVolume"); 
        
        _musicSlider.value = PlayerPrefs.HasKey("MusicVolume")
            ? _currentMusicVolume = PlayerPrefs.GetFloat("MusicVolume")
            : PlayerPrefs.GetFloat("MusicVolume");
        
        _sfxSlider.value = PlayerPrefs.HasKey("SFXVolume")
            ? _currentSfxVolume = PlayerPrefs.GetFloat("SFXVolume")
            : PlayerPrefs.GetFloat("SFXVolume"); 
    }

    public void StartLoadingLevel(int index)
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

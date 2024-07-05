using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : Singleton<MainMenu>
{
    public const int MAIN_MENU_SCENE_INDEX = 0;

    [Header("Menus")] [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _startMenu;
    [SerializeField] private GameObject _completeDayMenu;
    [SerializeField] private GameObject _sidebar;
    [SerializeField] private GameObject _levelMap;
    [SerializeField] private GameObject _options;
    [SerializeField] private GameObject _credits;
    [SerializeField] private GameObject _targetScreen;

    [Header("Button")] [SerializeField] private GameObject _btnMainMenu;
    [SerializeField] private GameObject _targetScreenButton;
    [SerializeField] private GameObject _continueButton;

    [Header("Audio")] [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private float _currentVolume;
    [SerializeField] private float _currentMusicVolume;
    [SerializeField] private float _currentSfxVolume;

    [Header("Images")] [SerializeField] private GameObject _backgroundImage;

    [Header("TMP Text ")] [SerializeField] private TMP_Text _completeDayText;
    [SerializeField] private TMP_Text _successText;
    [SerializeField] private TMP_Text _targetText;

    [Header("Stars & Bounty in Complete Day Menu")] [SerializeField]
    private GameObject _star1;
    [SerializeField] private GameObject _star2;
    [SerializeField] private GameObject _star3;

    [SerializeField] private GameObject _bounty1;
    [SerializeField] private GameObject _bounty2;
    [SerializeField] private GameObject _bounty3;

    [Header("Debug")] [SerializeField] private bool _startWithoutMenu;

    private bool _pausedGame;
    private bool _blockPauseMenu;
    private bool _levelLoading;

    public override void Awake()
    {
        InheritedDDoL = true;
        base.Awake();
    }

    public void Start()
    {
        _startMenu.SetActive(!_startWithoutMenu);

        LoadSettings();
        //  _backgroundImage.SetActive(true);

        /*
         * WIP: Used for Audio Control, later content!
         * -> NOPE! Keep in mind we already have an AudioManager
         * -> The main menu is not in charge to handle audio.
         * _backgroundAudio = FindObjectOfType<AudioSource>();
         * if(_backgroundAudio) _backgroundAudio.Play();
         */
    }

    public void StartGame()
    {
        _startMenu.SetActive(false);
        _levelMap.SetActive(true);
    }

    public void SetElementsForStart()
    {
        if (_completeDayMenu) _completeDayMenu.SetActive(false);
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

    public void TargetScreen()
    {
        if (Time.timeScale != 0f) Time.timeScale = 0.0f;
        if (!_targetScreen) _targetScreen.SetActive(true);
        var targetText = LevelManager.CurrentLevel.TargetText;
        _targetText.text = $"You must kill the {targetText} that comes into the restaurant.";
    }

    public void TargetScreenButton()
    {
        _targetScreen.SetActive(false);
        if (Time.timeScale != 1f) Time.timeScale = 1f;
        _sidebar.SetActive(true);
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
        if (_options) _options.SetActive(false);
        if (_credits) _credits.SetActive(false);
        if (_levelMap) _levelMap.SetActive(false);
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
        if (_sidebar) _sidebar.SetActive(false);
        _levelMap.SetActive(true);
        SceneManager.LoadScene(MAIN_MENU_SCENE_INDEX);
    }

    public void ReplayLevel()
    {
        var operation = LevelManager.Instance.LoadLevelAsync(LevelManager.CurrentLevelIndex);
        StartCoroutine(operation);
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR // UwU
        EditorApplication.ExitPlaymode();
#endif
    }

    public void CompleteDayMenu()
    {
        _btnMainMenu.SetActive(false);
        _completeDayMenu.SetActive(true);
        CalculateScore();
    }

    public void ContinueButton()
    {
        var operation = LevelManager.Instance.LoadLevelAsync(LevelManager.CurrentLevelIndex + 1);
        StartCoroutine(operation);
    }

    public void CalculateScore()
    {
        var starsAcquired = ProgressBar.Progress;

        if (starsAcquired > PlayerPrefs.GetInt("Stars" + LevelManager.CurrentLevelIndex))
            PlayerPrefs.SetInt("Stars" + LevelManager.CurrentLevelIndex, starsAcquired);

        if (starsAcquired >= 1)
        {
            if (LevelButton.UnlockedLevels == LevelManager.CurrentLevelIndex)
                LevelButton.UnlockedLevels++;

            PlayerPrefs.SetInt("UnlockedLevels", LevelButton.UnlockedLevels);

            _continueButton.SetActive(true);
            _completeDayText.text = "Congratulations, Level Completed!";
            _successText.text = "You completed day #" + (LevelManager.CurrentLevelIndex + 1).ToString().PadLeft(2, '0');
        }
        else
        {
            // Temporary for Gate I (Always succeed + pass to next level)
            _completeDayText.text = "To Bad you didn't pass this Level!";
            _successText.text = "You didn't completed day #" +
                                (LevelManager.CurrentLevelIndex + 1).ToString().PadLeft(2, '0');
            if (LevelButton.UnlockedLevels == LevelManager.CurrentLevelIndex) LevelButton.UnlockedLevels++;
            PlayerPrefs.SetInt("UnlockedLevels", LevelButton.UnlockedLevels);
        }

        _star1.SetActive(starsAcquired >= 1);
        _star2.SetActive(starsAcquired >= 2);
        _star3.SetActive(starsAcquired >= 3);

        //ToDo: Placeholder! Add Bounty Menu and connect to CalculateScore to Show correct Bountys in Menu after Completed Day , Reveal the right Target else let it Empty!
        if (_bounty1) _bounty1.SetActive(false);
        if (_bounty2) _bounty2.SetActive(false);
        if (_bounty3) _bounty3.SetActive(false);
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
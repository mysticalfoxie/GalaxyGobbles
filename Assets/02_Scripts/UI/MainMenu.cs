using System.Collections;
using System.Globalization;
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
    [SerializeField] private GameObject _creditsPage1;
    [SerializeField] private GameObject _creditsPage2;
    [SerializeField] private GameObject _creditsPage3;

    [Header("TMP Text ")] [SerializeField] private TMP_Text _completeDayText;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private TMP_Text _targetText;

    [Header("Stars & Bounty in Complete Day Menu")]
    [SerializeField] private GameObject _starRevealed1;
    [SerializeField] private GameObject _starRevealed2;
    [SerializeField] private GameObject _starRevealed3;
    [SerializeField] private GameObject _starUnrevealed1;
    [SerializeField] private GameObject _starUnrevealed2;
    [SerializeField] private GameObject _starUnrevealed3;

    [SerializeField] private GameObject _bounty1;
    [SerializeField] private GameObject _bounty2;
    [SerializeField] private GameObject _bounty3;

    [SerializeField] private TMP_Text _valueScore;
    [SerializeField] private TMP_Text _maxScore;
    
    [SerializeField] private GameObject _completeBountyStamp;
    [SerializeField] private GameObject _failedBountyStamp;
    [SerializeField] private GameObject _completeScoreStamp;
    [SerializeField] private GameObject _failedScoreStamp;


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
        if (_backgroundImage) _backgroundImage.SetActive(false);
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
        _backgroundImage.SetActive(true);
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

    public void Credits()
    {
        _credits.SetActive(true);
    }

    public void CreditsNextBtn()
    {
        if(_creditsPage1) _creditsPage2.SetActive(true);
    }

    public void CreditsBackBtn()
    {
        
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
        _backgroundImage.SetActive(true);
        AudioManager.Instance.StopAll();
        CalculateScore();
    }

    public void ContinueButton()
    {
        var operation = LevelManager.Instance.LoadLevelAsync(LevelManager.CurrentLevelIndex + 1);
        StartCoroutine(operation);
    }

    private void CalculateScore()
    {
        var starsAcquired = ProgressBar.Progress;
        var maxScore = LevelManager.CurrentLevel.MaxScore;
        _maxScore.text = Mathf.Floor(maxScore).ToString(CultureInfo.InvariantCulture);
        _valueScore.text = Mathf.Floor(BottomBar.Instance.Score.Value).ToString(CultureInfo.InvariantCulture);
        _levelText.text = "Level" + (LevelManager.CurrentLevelIndex + 1).ToString().PadLeft(2, '0');
        
        if (starsAcquired > PlayerPrefs.GetInt("Stars" + LevelManager.CurrentLevelIndex))
            PlayerPrefs.SetInt("Stars" + LevelManager.CurrentLevelIndex, starsAcquired);

        if (starsAcquired >= 1)
        {
            if (LevelButton.UnlockedLevels == LevelManager.CurrentLevelIndex)
                LevelButton.UnlockedLevels++;

            PlayerPrefs.SetInt("UnlockedLevels", LevelButton.UnlockedLevels);

            _continueButton.SetActive(true);
            if (_failedScoreStamp) _failedScoreStamp.SetActive(false);
            _completeScoreStamp.SetActive(true);
            
            AudioManager.Instance.PlayMusic(AudioSettings.Data.WinMusic);
            //_completeDayText.text = "You completed day #" + (LevelManager.CurrentLevelIndex + 1).ToString().PadLeft(2, '0');  [ToDO Maybe need later...]
        }
        else
        {
            // Temporary for Gate I (Always succeed + pass to next level)
            //_completeDayText.text = "You didn't pass this Level!"; [ToDO Maybe need later...]
            if (_completeScoreStamp) _completeScoreStamp.SetActive(false);
            _failedScoreStamp.SetActive(true);
            if (_continueButton) _continueButton.SetActive(false);
            if (LevelButton.UnlockedLevels == LevelManager.CurrentLevelIndex) LevelButton.UnlockedLevels++;
            PlayerPrefs.SetInt("UnlockedLevels", LevelButton.UnlockedLevels);
            
            AudioManager.Instance.PlayMusic(AudioSettings.Data.LooseMusic);
        }

        _starRevealed1.SetActive(starsAcquired >= 1);
        _starRevealed2.SetActive(starsAcquired >= 2);
        _starRevealed3.SetActive(starsAcquired >= 3);

        //ToDo: Placeholder! Add Bounty Menu and connect to CalculateScore to Show correct Bounty in Menu after Completed Day , Reveal the right Target else let it Empty!
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

    private void Save()
    {
        PlayerPrefs.SetFloat("MasterVolume", _currentVolume);
        PlayerPrefs.SetFloat("MusicVolume", _currentMusicVolume);
        PlayerPrefs.SetFloat("SFXVolume", _currentSfxVolume);
    }

    private void LoadSettings()
    {
        _volumeSlider.value = PlayerPrefs.HasKey("MasterVolume")
            ? _currentVolume = PlayerPrefs.GetFloat("MasterVolume")
            : PlayerPrefs.GetFloat("MasterVolume");
        _audioMixer.SetFloat("MasterVolume", _currentVolume);

        _musicSlider.value = PlayerPrefs.HasKey("MusicVolume")
            ? _currentMusicVolume = PlayerPrefs.GetFloat("MusicVolume")
            : PlayerPrefs.GetFloat("MusicVolume");
        _audioMixer.SetFloat("MusicVolume", _currentMusicVolume);

        _sfxSlider.value = PlayerPrefs.HasKey("SFXVolume")
            ? _currentSfxVolume = PlayerPrefs.GetFloat("SFXVolume")
            : PlayerPrefs.GetFloat("SFXVolume");
        _audioMixer.SetFloat("SFXVolume", _currentSfxVolume);
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
        yield return Fader.Instance.FadeBlackAsync();

        // Before the new level has started loading  
        yield return LevelManager.Instance.LoadLevelAsync(index);
        // After the level has completely loaded

        // When it's still black, show the bounty screen, so this is the first thing the users sees after fading back.
        // TODO: Show Bounty Screen

        yield return Fader.Instance.FadeWhiteAsync();

        // When done with level loading the button becomes clickable again
        // -> Stay at the last line of the function
        _levelLoading = false;
    }
}

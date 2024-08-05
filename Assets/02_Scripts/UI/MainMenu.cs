using System;
using System.Collections;
using System.Globalization;
using System.Linq;
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
    [SerializeField] private GameObject _assassinationBriefing;

    [Header("Button")] [SerializeField] private GameObject _btnMainMenu;
    [SerializeField] private GameObject _continueButton;

    [Header("Audio")] [SerializeField] private AudioMixer _audioMixer;
    [SerializeField] private Slider _volumeSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private Slider _sfxSlider;
    [SerializeField] private float _currentVolume;
    [SerializeField] private float _currentMusicVolume;
    [SerializeField] private float _currentSfxVolume;

    [Header("Images")] [SerializeField] private GameObject _backgroundImage;
    [SerializeField] private Sprite _ikaruzBountySuccess;
    [SerializeField] private Sprite _ikaruzBountyFail;
    [SerializeField] private Sprite _bobBountySuccess;
    [SerializeField] private Sprite _bobBountyFail;
    [SerializeField] private Sprite _broccoloidBountySuccess;
    [SerializeField] private Sprite _broccoloidBountyFail;
    [SerializeField] private Sprite _emptyBounty;

    [Header("TMP Text ")] [SerializeField] private TMP_Text _completeDayText;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private TMP_Text _targetText;
    [SerializeField] private TMP_Text _minScoreText;
    [SerializeField] private TMP_Text _levelNumberAssassinationBriefing;
    [SerializeField] private TMP_Text _noDeathsText;

    [Header("Assassination Briefing")] [SerializeField]
    private GameObject _ikaruzCard;

    [SerializeField] private GameObject _bobCard;
    [SerializeField] private GameObject _broccoloidCard;

    [Header("Stars & Bounty in Complete Day Menu")] [SerializeField]
    private GameObject _starRevealed1;

    [SerializeField] private GameObject _starRevealed2;
    [SerializeField] private GameObject _starRevealed3;
    [SerializeField] private GameObject _starUnrevealed1;
    [SerializeField] private GameObject _starUnrevealed2;
    [SerializeField] private GameObject _starUnrevealed3;

    [SerializeField] private Image _bounty1;
    [SerializeField] private Image _bounty2;
    [SerializeField] private Image _bounty3;

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
    private bool _assassinationBriefingLoading;

    public override void Awake()
    {
        InheritedDDoL = true;
        base.Awake();
    }

    public void Start()
    {
        _startMenu.SetActive(!_startWithoutMenu);

        LoadSettings();
    }

    public void StartGame()
    {
        _startMenu.SetActive(false);
        _levelMap.SetActive(true);
    }

    public void SetElementsForStart()
    {
        if (_completeDayMenu) _completeDayMenu.SetActive(false);
        Time.timeScale = 1.0f;
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
        if (_assassinationBriefing) _assassinationBriefing.SetActive(false);
        Time.timeScale = 1.0f;
        if (_pauseMenu) _pauseMenu.SetActive(false);
        _backgroundImage.SetActive(true);
        _startMenu.SetActive(true);
        _blockPauseMenu = true;
        _sidebar.SetActive(false);
        SceneManager.LoadScene(MAIN_MENU_SCENE_INDEX);
    }

    public void AssassinationBriefing()
    {
        if (LevelManager.CurrentLevel.Target is null) return;
        _assassinationBriefing.SetActive(true);
        _levelNumberAssassinationBriefing.text = $"Level {LevelManager.CurrentLevel.Number.ToString().PadLeft(2, '0')}";
        _targetText.text = $"{LevelManager.CurrentLevel.TargetPosition.ToPositionString()} {LevelManager.CurrentLevel.Target.Name}";
        _minScoreText.text = Mathf.FloorToInt(LevelManager.CurrentLevel.MinScore).ToString();
        foreach (var image in new[] { _ikaruzCard, _bobCard, _broccoloidCard }) image.SetActive(false);
        if (LevelManager.CurrentLevel.Target.name == Identifiers.Value.Ikaruz.name) _ikaruzCard.SetActive(true);
        if (LevelManager.CurrentLevel.Target.name == Identifiers.Value.Bob.name) _bobCard.SetActive(true);
        if (LevelManager.CurrentLevel.Target.name == Identifiers.Value.Broccoloid.name) _broccoloidCard.SetActive(true);
    }

    public void AssassinationBriefingContinue()
    {
        if (_assassinationBriefingLoading) return;
        _assassinationBriefingLoading = true;
        GlobalTimeline.Instance.Disable();
        Time.timeScale = 1F;

        StartCoroutine(Fader.Instance.FadeBlackWhiteWhile(
            () => _assassinationBriefing.SetActive(false),
            () =>
            {
                _assassinationBriefingLoading = false;
                GlobalTimeline.Instance.Enable();
            }));
    }

    public void AssassinationBriefingBack()
    {
        if (_assassinationBriefingLoading) return;
        _assassinationBriefingLoading = true;
        Time.timeScale = 1F;
        GlobalTimeline.Instance.Disable();
        StartCoroutine(Fader.Instance.FadeBlackWhiteWhile(HomeMenu,
            () => _assassinationBriefingLoading = false));
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
        Time.timeScale = 1.0f;
        if (_pauseMenu) _pauseMenu.SetActive(false);
        if (_blockPauseMenu == false) _blockPauseMenu = true;
        if (_sidebar) _sidebar.SetActive(false);
        _levelMap.SetActive(true);
        SceneManager.LoadScene(MAIN_MENU_SCENE_INDEX);
    }

    public void ReplayLevel()
    {
        StartLoadingLevel(LevelManager.CurrentLevelIndex);
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
        RenderBounties();
    }

    public void ContinueButton()
    {
        StartLoadingLevel(LevelManager.CurrentLevelIndex + 1);
    }

    private void RenderBounties()
    {
        if (LevelManager.CurrentLevel.Target is null)
        {
            _bounty1.transform.gameObject.SetActive(false);
            _bounty2.transform.gameObject.SetActive(false);
            _bounty3.transform.gameObject.SetActive(false); 
            _noDeathsText.gameObject.SetActive(true);
            _completeBountyStamp.SetActive(false);
            _failedBountyStamp.SetActive(false);
        }
        else
        { 
            _noDeathsText.gameObject.SetActive(false);
            _bounty1.transform.gameObject.SetActive(true);
            _bounty2.transform.gameObject.SetActive(true);
            _bounty3.transform.gameObject.SetActive(true); 
            var bounties = BottomBar.Instance.Bounties.GetBounties();
            _bounty1.sprite = GetBountyCard(bounties, 0);
            _bounty2.sprite = GetBountyCard(bounties, 1);
            _bounty3.sprite = GetBountyCard(bounties, 2);
            var bountySucceeded = bounties.Any(x => x.WasTarget);
            _completeBountyStamp.SetActive(bountySucceeded);
            _failedBountyStamp.SetActive(!bountySucceeded);
            if (_continueButton.activeInHierarchy)
                _continueButton.SetActive(bountySucceeded);
        }
    }

    private Sprite GetBountyCard(BountyData[] bounties, int index)
    {
        if (index > bounties.Length - 1)
            return _emptyBounty;
        
        if (bounties[index].WasTarget)
        {
            if (bounties[index].Species.name == Identifiers.Value.Broccoloid.name)
                return _broccoloidBountySuccess;
            if (bounties[index].Species.name == Identifiers.Value.Bob.name)
                return _bobBountySuccess;
            if (bounties[index].Species.name == Identifiers.Value.Ikaruz.name)
                return _ikaruzBountySuccess;
            
            throw new NotSupportedException($"Could not find a bounty card for species \"{bounties[index].Species.name}\".");
        }

        if (bounties[index].Species.name == Identifiers.Value.Broccoloid.name)
            return _broccoloidBountyFail;
        if (bounties[index].Species.name == Identifiers.Value.Bob.name)
            return _bobBountyFail;
        if (bounties[index].Species.name == Identifiers.Value.Ikaruz.name)
            return _ikaruzBountyFail;
        
        throw new NotSupportedException($"Could not find a bounty card for species \"{bounties[index].Species.name}\".");
    }

    private void CalculateScore()
    {
        var starsAcquired = ProgressBar.Progress;
        var maxScore = LevelManager.CurrentLevel.MaxScore;
        _maxScore.text = Mathf.Floor(maxScore).ToString(CultureInfo.InvariantCulture);
        _valueScore.text = Math.Ceiling(BottomBar.Instance.Score.Value).ToString(CultureInfo.InvariantCulture);
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
        }
        else
        {
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
        if (_levelLoading) return;
        _levelLoading = true;

        StartCoroutine(LoadLevelAsync(index));
    }

    private IEnumerator LoadLevelAsync(int index)
    {
        yield return Fader.Instance.FadeBlackAsync();
        yield return LevelManager.Instance.LoadLevelAsync(index);
        AssassinationBriefing();
        yield return Fader.Instance.FadeWhiteAsync();
        if (_assassinationBriefing.activeInHierarchy) Time.timeScale = 0F;
        _levelLoading = false;
    }
}

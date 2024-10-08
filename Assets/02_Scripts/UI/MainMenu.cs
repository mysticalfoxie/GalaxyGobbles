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

    [Header("Menus")] 
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private GameObject _pauseMenuPanel;
    [SerializeField] private GameObject _startMenu;
    [SerializeField] private GameObject _completeDayMenu;
    [SerializeField] private GameObject _sidebar;
    [SerializeField] private GameObject _levelMap;
    [SerializeField] private GameObject _options;
    [SerializeField] private GameObject _howToPlay;
    [SerializeField] private GameObject _credits;
    [SerializeField] private GameObject _assassinationBriefing;
    [SerializeField] private LevelSelector _levelSelector;

    [Header("Button")] [SerializeField] private GameObject _btnMainMenu;
    [SerializeField] private GameObject _continueButton;
    [SerializeField] private GameObject _creditsButton;

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

    [Header("TMP Text")] 
    [SerializeField] private TMP_Text _completeDayText;
    [SerializeField] private TMP_Text _levelText;
    [SerializeField] private TMP_Text _targetText;
    [SerializeField] private TMP_Text _minScoreText;
    [SerializeField] private TMP_Text _levelNumberAssassinationBriefing;
    [SerializeField] private TMP_Text _noDeathsText;

    [Header("Assassination Briefing")] 
    [SerializeField] private GameObject _ikaruzCard;

    [SerializeField] private GameObject _bobCard;
    [SerializeField] private GameObject _broccoloidCard;

    [Header("Stars & Bounty in Complete Day Menu")] 
    [SerializeField] private GameObject _starRevealed1;
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

    private bool _blockPauseMenu;
    private bool _levelLoading;
    private bool _assassinationBriefingLoading;
    private bool _optionsOriginIsMenu;

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
        AudioManager.Instance.PlaySFX(AudioSettings.Data.UIOpenPopup);
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
        AudioManager.Instance.PlaySFX(AudioSettings.Data.UIOpenPopup);
        Time.timeScale = 0.0f;
        _pauseMenu.SetActive(true);
    }

    public void ResumeGame()
    {
        AudioManager.Instance.PlaySFX(AudioSettings.Data.UIClose);
        Time.timeScale = 1.0f;
        _pauseMenu.SetActive(false);
    }

    public void HomeMenu(bool skipLoad = false)
    {
        if (skipLoad)
            HomeMenuInternal();
        else
            StartLoadingLevel(-1, HomeMenuInternal);
    }

    private void HomeMenuInternal()
    {
        AudioManager.Instance.PlaySFX(AudioSettings.Data.UIClose);
        if (_completeDayMenu) _completeDayMenu.SetActive(false);
        if (_credits) _credits.SetActive(false);
        if (_howToPlay) _howToPlay.SetActive(false);
        if (_assassinationBriefing) _assassinationBriefing.SetActive(false);
        Time.timeScale = 1.0f;
        if (_pauseMenu) _pauseMenu.SetActive(false);
        _backgroundImage.SetActive(true);
        _startMenu.SetActive(true);
        _blockPauseMenu = true;
        _sidebar.SetActive(false);
    }

    public void AssassinationBriefing()
    {
        if (LevelManager.CurrentLevel.Target is null) return;
        GlobalTimeline.Instance.Disable();
        _assassinationBriefing.SetActive(true);
        _levelNumberAssassinationBriefing.text = $"Level {LevelManager.CurrentLevel.Number.ToString().PadLeft(2, '0')}";
        _targetText.text =
            $"{LevelManager.CurrentLevel.TargetPosition.ToPositionString()} {LevelManager.CurrentLevel.Target.Name}";
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
        AudioManager.Instance.PlaySFX(AudioSettings.Data.UIStartGame);
    }

    public void AssassinationBriefingBack()
    {
        if (_assassinationBriefingLoading) return;
        _assassinationBriefingLoading = true;
        AudioManager.Instance.PlaySFX(AudioSettings.Data.UIBack);
        Time.timeScale = 1F;
        GlobalTimeline.Instance.Disable();
        StartCoroutine(Fader.Instance.FadeBlackWhiteWhile(
            () => HomeMenu(),
            () => _assassinationBriefingLoading = false));
    }

    public void Options(bool originIsMenu)
    {
        AudioManager.Instance.PlaySFX(AudioSettings.Data.UIOpenPopup);
        _optionsOriginIsMenu = originIsMenu;
        _options.SetActive(true);
        if (!_optionsOriginIsMenu)
            _pauseMenuPanel.SetActive(false);
    }

    public void HowToPlay()
    {
        AudioManager.Instance.PlaySFX(AudioSettings.Data.UIOpenPopup);
        _howToPlay.SetActive(true);
        _startMenu.SetActive(false);
    }

    public void Credits()
    {
        AudioManager.Instance.PlaySFX(AudioSettings.Data.UIOpenPopup);
        if (SceneManager.GetActiveScene().buildIndex == MAIN_MENU_SCENE_INDEX)
        {
            _credits.SetActive(true);
            _startMenu.SetActive(false);
            return;
        }

        Time.timeScale = 1F;
        StartCoroutine(Fader.Instance.FadeBlackWhiteWhile(
            () =>
            {
                _completeDayMenu.SetActive(false);
                SceneManager.LoadScene(MAIN_MENU_SCENE_INDEX);
                _credits.SetActive(true);
                _startMenu.SetActive(false);
            }));
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
        AudioManager.Instance.PlaySFX(AudioSettings.Data.UIBack);
        if (_options) _options.SetActive(false);
        if (_credits) _credits.SetActive(false);
        if (_levelMap) _levelMap.SetActive(false);
        _startMenu.SetActive(true);
    }

    public void BackToLevelSelection()
    {
        AudioManager.Instance.PlaySFX(AudioSettings.Data.UIBack);
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
        AudioManager.Instance.PlaySFX(AudioSettings.Data.UIStartGame);
        if (_pauseMenu) _pauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        StartLoadingLevel(LevelManager.CurrentLevelIndex);
    }

    public void QuitGame()
    {
        AudioManager.Instance.PlaySFX(AudioSettings.Data.UIClose);
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

        UpdateProgress();

        AudioManager.Instance.PlayMusic(_continueButton.activeInHierarchy
            ? AudioSettings.Data.WinMusic
            : AudioSettings.Data.LooseMusic);
    }

    private void UpdateProgress()
    {
        var winByScore = CalculateScore();
        var winByBounties = RenderBounties();
        var succeeded = winByBounties && winByScore;
        var lastLevel = LevelManager.CurrentLevel.Number == GameSettings.Data.Levels.Max(x => x.Number);

        DataManager.UpdateProgress(LevelManager.CurrentLevel.Number, ProgressBar.Progress, succeeded);
        _levelSelector.UpdateLevels();
        
        _starRevealed1.SetActive(false);
        _starRevealed2.SetActive(false);
        _starRevealed3.SetActive(false);
        _continueButton.SetActive(succeeded && !lastLevel);
        _creditsButton.SetActive(succeeded && lastLevel);
        if (succeeded)
            StartCoroutine(PlayStarsAnimation(ProgressBar.Progress));
    }

    public void ContinueButton()
    {
        AudioManager.Instance.PlaySFX(AudioSettings.Data.UIStartGame);
        StartLoadingLevel(LevelManager.CurrentLevelIndex + 1);
    }

    private bool RenderBounties()
    {
        var bountyObjects = new[] { _bounty1, _bounty2, _bounty3 };
        foreach (var bounty in bountyObjects)
            bounty.gameObject.SetActive(LevelManager.CurrentLevel.Target is not null);
        
        // No assassination target existed
        if (LevelManager.CurrentLevel.Target is null)
        {
            _noDeathsText.gameObject.SetActive(true);
            _completeBountyStamp.SetActive(false);
            _failedBountyStamp.SetActive(false);
            return true;
        }

        var bounties = BottomBar.Instance.Bounties.GetBounties();
        for (var i = 0; i < bountyObjects.Length; i++)
            bountyObjects[i].sprite = GetBountyCard(bounties, i);

        var bountySucceeded = bounties.Any(x => x.WasTarget);
        _completeBountyStamp.SetActive(bountySucceeded);
        _failedBountyStamp.SetActive(!bountySucceeded);
        _noDeathsText.gameObject.SetActive(false);

        return bountySucceeded;
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

            throw new NotSupportedException(
                $"Could not find a bounty card for species \"{bounties[index].Species.name}\".");
        }

        if (bounties[index].Species.name == Identifiers.Value.Broccoloid.name)
            return _broccoloidBountyFail;
        if (bounties[index].Species.name == Identifiers.Value.Bob.name)
            return _bobBountyFail;
        if (bounties[index].Species.name == Identifiers.Value.Ikaruz.name)
            return _ikaruzBountyFail;

        throw new NotSupportedException(
            $"Could not find a bounty card for species \"{bounties[index].Species.name}\".");
    }

    private bool CalculateScore()
    {
        var starsAcquired = ProgressBar.Progress;
        var maxScore = LevelManager.CurrentLevel.MaxScore;
        _maxScore.text = Mathf.Floor(maxScore).ToString(CultureInfo.InvariantCulture);
        _valueScore.text = Math.Ceiling(BottomBar.Instance.Score.Value).ToString(CultureInfo.InvariantCulture);
        _levelText.text = $"Level {LevelManager.CurrentLevel.Number}";

        if (starsAcquired >= 1)
            OnLevelSucceed();
        else
            OnLevelFailed();

        return starsAcquired >= 1;
    }

    private IEnumerator PlayStarsAnimation(int starsAcquired)
    {
        yield return new WaitForSecondsRealtime(GameSettings.Data.StarsAnimationDelay);
        
        var audios = new[]
        {
            AudioSettings.Data.UIStarComboOne,
            AudioSettings.Data.UIStarComboTwo,
            AudioSettings.Data.UIStarComboThree,
        };

        for (var i = 0; i < starsAcquired; i++)
        {
            _starRevealed1.SetActive(i >= 0);
            _starRevealed2.SetActive(i >= 1);
            _starRevealed3.SetActive(i >= 2);
            AudioManager.Instance.PlaySFX(audios[i]);
            yield return new WaitForSecondsRealtime(audios[i].Source.length - 1F);
        }
    }

    private void OnLevelFailed()
    {
        if (_completeScoreStamp) _completeScoreStamp.SetActive(false);
        _failedScoreStamp.SetActive(true);
    }

    private void OnLevelSucceed()
    {
        if (_failedScoreStamp) _failedScoreStamp.SetActive(false);
        _completeScoreStamp.SetActive(true);
    }

    public void ApplyOptions()
    {
        AudioManager.Instance.PlaySFX(AudioSettings.Data.UIClose);
        Save();

        if (_optionsOriginIsMenu)
        {
            BackButton();
            return;
        }

        _pauseMenuPanel.SetActive(true);
        _options.SetActive(false);
    }

    private void Save()
    {
        AudioManager.Instance.PlaySFX(AudioSettings.Data.UIClose);
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

    public void StartLoadingLevel(int index, Action actionsDuringBlack = null)
    {
        if (_levelLoading) return;
        _levelLoading = true;

        StartCoroutine(LoadLevelAsync(index, actionsDuringBlack));
    }

    private IEnumerator LoadLevelAsync(int index, Action actionsDuringBlack = null)
    {
        Time.timeScale = 1F;
        yield return Fader.Instance.FadeBlackAsync();

        if (index == -1)
            yield return LoadSceneAsyncInternal(MAIN_MENU_SCENE_INDEX);
        else
            yield return LoadLevelAsyncInternal(index);

        actionsDuringBlack?.Invoke();

        yield return Fader.Instance.FadeWhiteAsync();
        _levelLoading = false;
    }

    private static IEnumerator LoadSceneAsyncInternal(int index)
    {
        yield return SceneManager.LoadSceneAsync(index);
    }

    private IEnumerator LoadLevelAsyncInternal(int index)
    {
        yield return LevelManager.Instance.LoadLevelAsync(index);
        AssassinationBriefing();
    }
}

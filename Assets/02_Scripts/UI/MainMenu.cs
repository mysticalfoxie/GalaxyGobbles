using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;
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

    
    private bool _pausedGame;
    private bool _blockPauseMenu;
    public static MainMenu Instance { get; private set; } 

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

    public void Options()
    {
        _options.SetActive(true);
    }

    public void SetVolume(float volume)
    {
        _audioMixer.SetFloat("Volume", volume);
        _currentVolume = volume;
    }
    public void SetMusic(float music)
    {
        _audioMixer.SetFloat("Music", music);
        _currentMusicVolume = music;
    }
    public void SetSfx(float sfx)
    {
        _audioMixer.SetFloat("SFX", sfx);
        _currentSfxVolume = sfx;
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
        EditorApplication.ExitPlaymode(); // hab ich grad online gefunden ;) 
    }

    public void CompleteDay()
    {
        _btnMainMenu.SetActive(false);
        _completeDayMenu.SetActive(true);
    }

    public void BackAndSave()
    {
        Save();
        _options.SetActive(false);
        if (_pausedGame) ResumeGame();
        else BackButton();
    }
    public void Save()
    {
        PlayerPrefs.SetFloat("VolumePref", _currentVolume);
        PlayerPrefs.SetFloat("MusicPref", _currentMusicVolume);
        PlayerPrefs.SetFloat("SFXPref", _currentSfxVolume);
    }

    public void LoadSettings()
    {
        _volumeSlider.value = PlayerPrefs.HasKey("VolumePref")
            ? _currentVolume = PlayerPrefs.GetFloat("VolumePref")
            : PlayerPrefs.GetFloat("VolumePref"); 
        
        _musicSlider.value = PlayerPrefs.HasKey("MusicPref")
            ? _currentMusicVolume = PlayerPrefs.GetFloat("MusicPref")
            : PlayerPrefs.GetFloat("MusicPref");
        
        _sfxSlider.value = PlayerPrefs.HasKey("SFXPref")
            ? _currentSfxVolume = PlayerPrefs.GetFloat("SFXPref")
            : PlayerPrefs.GetFloat("SFXPref"); 
    }
}

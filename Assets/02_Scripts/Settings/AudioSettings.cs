using System;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "CFG_Audio Settings", menuName = "Galaxy Gobbles/Configurations/Audio Settings", order = 2)]
public class AudioSettings : ScriptableObject
{
    #region Serialized Fields

    [Header("Music")] 
    [SerializeField] private AudioData _uiMusic;
    [SerializeField] private AudioData _levelMusic;
    [SerializeField] private AudioData _tensionMusic;
    [SerializeField] private AudioData _winMusic;
    [SerializeField] private AudioData _looseMusic;

    [Header("SFX")] 
    [SerializeField] private AudioData _customerEnters;
    [SerializeField] private AudioData _customerBeaming;
    [SerializeField] private AudioData _brocoloidVoiceLove;
    [SerializeField] private AudioData _brocoloidVoiceHappy;
    [SerializeField] private AudioData _brocoloidVoiceAngry;
    [SerializeField] private AudioData _icarusVoiceLove;
    [SerializeField] private AudioData _icarusVoiceHappy;
    [SerializeField] private AudioData _icarusVoiceAngry;
    [SerializeField] private AudioData _bobVoiceLove;
    [SerializeField] private AudioData _bobVoiceHappy;
    [SerializeField] private AudioData _bobVoiceAngry;
    [SerializeField] private AudioData _puffChairDrop;
    [SerializeField] private AudioData _click;
    [SerializeField] private AudioData _eatingSound;
    [SerializeField] private AudioData _moneyPaid;
    [SerializeField] private AudioData _fartNoise;
    [SerializeField] private AudioData _laserScanCleaning;
    [SerializeField] private AudioData _errorNah;
    [SerializeField] private AudioData _clickFood;
    [SerializeField] private AudioData _boilingSounds;
    [SerializeField] private AudioData _ready;
    [SerializeField] private AudioData _overcooked;
    [SerializeField] private AudioData _burning;
    [SerializeField] private AudioData _ingredientEyeAdded;
    [SerializeField] private AudioData _ingredientSquidAdded;
    [SerializeField] private AudioData _ingredientBokChoyAdded;
    [SerializeField] private AudioData _trashSound;
    [SerializeField] private AudioData _poisonIsAdded;
    [SerializeField] private AudioData _teaIsFilling;
    [SerializeField] private AudioData _teaIsReady;
    [SerializeField] private AudioData _clickDrink;
    [SerializeField] private AudioData _ambientSound;
    [SerializeField] private AudioData _potCleaning;
    [SerializeField] private AudioData _takeReadyNoodles;

    #endregion

    #region Properties

    public AudioData TensionMusic => _tensionMusic;
    public AudioData UiMusic => _uiMusic;
    public AudioData LevelMusic => _levelMusic;
    public AudioData WinMusic => _winMusic;
    public AudioData LooseMusic => _looseMusic;
    public AudioData CustomerEnters => _customerEnters;
    public AudioData CustomerBeaming => _customerBeaming;
    public AudioData BrocoloidVoiceLove => _brocoloidVoiceLove;
    public AudioData BrocoloidVoiceHappy => _brocoloidVoiceHappy;
    public AudioData BrocoloidVoiceAngry => _brocoloidVoiceAngry;
    public AudioData IcarusVoiceLove => _icarusVoiceLove;
    public AudioData IcarusVoiceHappy => _icarusVoiceHappy;
    public AudioData IcarusVoiceAngry => _icarusVoiceAngry;
    public AudioData BobVoiceLove => _bobVoiceLove;
    public AudioData BobVoiceHappy => _bobVoiceHappy;
    public AudioData BobVoiceAngry => _bobVoiceAngry;
    public AudioData PuffChairDrop => _puffChairDrop;
    public AudioData Click => _click;
    public AudioData EatingSound => _eatingSound;
    public AudioData MoneyPaid => _moneyPaid;
    public AudioData FartNoise => _fartNoise;
    public AudioData LaserScanCleaning => _laserScanCleaning;
    public AudioData ErrorNah => _errorNah;
    public AudioData ClickFood => _clickFood;
    public AudioData BoilingSounds => _boilingSounds;
    public AudioData Ready => _ready;
    public AudioData Overcooked => _overcooked;
    public AudioData Burning => _burning;
    public AudioData IngredientEyeAdded => _ingredientEyeAdded;
    public AudioData IngredientSquidAdded => _ingredientSquidAdded;
    public AudioData IngredientBokChoyAdded => _ingredientBokChoyAdded;
    public AudioData TrashSound => _trashSound;
    public AudioData PoisonIsAdded => _poisonIsAdded;
    public AudioData TeaIsFilling => _teaIsFilling;
    public AudioData TeaIsReady => _teaIsReady;
    public AudioData ClickDrink => _clickDrink;
    public AudioData AmbientSound => _ambientSound;
    public AudioData PotCleaning => _potCleaning;
    public AudioData TakeReadyNoodles => _takeReadyNoodles;
    
    #endregion

    #region Boilerplate

    public const string SETTINGS_PATH = "Assets/10_Miscellaneous/03_Settings/CFG_Audio Settings.asset";

    private static AudioSettings _data;
    public static AudioSettings Data => _data ??= GetSettings();

    internal static AudioSettings GetSettings()
    {
#if UNITY_EDITOR
        var settings = AssetDatabase.LoadAssetAtPath<AudioSettings>(SETTINGS_PATH);
        if (settings != null) return settings;

        throw new Exception("Could not find the GameSettings!");
#else
        return References.Instance.GetAudioSettings();
#endif
    }

    public static AudioData GetTrackBySceneIndex(int index) 
        => index switch
        {
            0 => Data.UiMusic,
            1 => Data.LevelMusic,
            _ => null
        };

    #endregion
}
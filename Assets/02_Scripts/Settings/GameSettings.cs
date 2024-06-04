using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

// ReSharper disable InconsistentNaming

public class GameSettings : ScriptableObject
{
    [Header("General Settings")]
    [Tooltip("The amount of time it takes for the noodles to cook.")]
    [SerializeField] private float _noodleBoilingTime = 5.0F;
    [Tooltip("The amount of time it takes for the character to clean the overcooked noodle pot.")]
    [SerializeField] private float _potCleaningTime = 0.2F;
    [Tooltip("The amount of time it takes for the noodles to overcook. The timer starts after the noodles were cooked.")]
    [SerializeField] private float _noodleOvercookTime = 5.0F;
    [Tooltip("The amount of time it takes for the customer to think about his meal.")]
    [SerializeField] private float _customerThinkingTime = 3.0F;
    [Tooltip("The amount of time it takes for the customer to eat.")]
    [SerializeField] private float _customerEatingTime = 3.0F;
    [Tooltip("The amount of time it takes for the customer to die from poison.")]
    [SerializeField] private float _customerDyingTime = 3.0F;
    [Tooltip("The amount of time it takes for the customer to stock up the queue, when the one in front is seated.")]
    [SerializeField] private float _queueRestockDelay = 0.2F;
    [Tooltip("The amount of time it takes for the poison cloud to disappear.")]
    [SerializeField] private float _poisonHideDelay = 2.0F;
    [Tooltip("The amount of time between the customers poisoning and his killing animation (Poison Cloud).")]
    [SerializeField] private float _customerKillDelay = 2.0F;

    [Header("Rendering")] 
    [Tooltip("The poison icon that should be added to an item.")]
    [SerializeField] private SpriteData _poisonIcon;
    
    [Header("References")]
    [Header("Prefabs")]
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private GameObject _customerPrefab;
    [SerializeField] private GameObject _spriteRendererPrefab;
    [SerializeField] private GameObject _rectTransformPrefab;

    [Header("Music")] 
    [SerializeField] private AudioData _mainMenuMusic;
    [SerializeField] private AudioData _inGameMusic;

    [Header("Game Data")] 
    [Tooltip("When this is enabled, unity will automatically map all customers to the levels, and gather all references for the next entries automatically.")]
    [SerializeField] private bool _autoMap;
    [Tooltip("When this is enabled, unity will also clear the console every time this happens to avoid spamming the console full.")]
    [SerializeField] private bool _autoMapClearsConsole;
    [SerializeField] private LevelData[] _levels;
    [SerializeField] private SpeciesData[] _species;
    [SerializeField] private ItemData[] _items;
    [SerializeField] private RecipeData[] _recipes;
    
    #region Properties
    public float NoodleBoilingTime => _noodleBoilingTime;
    public float PotCleaningTime => _potCleaningTime;
    public float NoodleOvercookTime => _noodleOvercookTime;
    public float CustomerThinkingTime => _customerThinkingTime;
    public float CustomerDyingTime => _customerDyingTime;
    public float CustomerEatingTime => _customerEatingTime;
    public float QueueRestockDelay => _queueRestockDelay;
    public float PoisonHideDelay => _poisonHideDelay;
    public float CustomerKillDelay => _customerKillDelay;

    public SpriteData PoisonIcon => _poisonIcon;
    
    public GameObject PRE_Item => _itemPrefab;
    public GameObject PRE_SpriteRenderer => _spriteRendererPrefab;
    public GameObject PRE_Customer => _customerPrefab;
    public GameObject PRE_RectTransform => _rectTransformPrefab;
    
    public IEnumerable<LevelData> Levels => _levels;
    public IEnumerable<SpeciesData> Species => _species;
    public IEnumerable<ItemData> Items => _items;
    public IEnumerable<RecipeData> Recipes => _recipes;
    
    public AudioData MainMenuMusic => _mainMenuMusic;
    public AudioData InGameMusic => _inGameMusic;
    #endregion
    
    #region Boilerplate
    public const string SETTINGS_PATH = "Assets/10_Miscellaneous/03_Settings/CFG_Game Settings.asset";
    
    private static GameSettings _data;
    public static GameSettings Data => _data ??= GetOrCreateSettings();

    internal static GameSettings GetOrCreateSettings()
    {
#if UNITY_EDITOR
        var settings = AssetDatabase.LoadAssetAtPath<GameSettings>(SETTINGS_PATH);
        if (settings != null) return settings;
        
        settings = CreateDefaultSettings();
        AssetDatabase.CreateAsset(settings, SETTINGS_PATH);
        AssetDatabase.SaveAssets();
        
        return settings;
#else
        return References.Instance.GetLocalSettings();
#endif
    }

    private static GameSettings CreateDefaultSettings()
    {
        var settings = CreateInstance<GameSettings>();
        settings._species = Array.Empty<SpeciesData>();
        settings._levels = Array.Empty<LevelData>();
        settings._items = Array.Empty<ItemData>();
        return settings;
    }
    #endregion
    
    #region Utilities

    private DateTime _lastMapping;
    
#if UNITY_EDITOR
    public void OnEnable()
    {
        if (!_autoMap) return;
        
        // Unity tends to call the Enable function twice or even more.
        // This causes errors when saving the asset database multiple times within a short timespan.
        // So we only allow mapping once within 5 seconds 
        if (_lastMapping > DateTime.Now.AddSeconds(-5)) return;
        _lastMapping = DateTime.Now;

        if (_autoMapClearsConsole)
        {
            DebugUtils.ClearConsole();
            Debug.Log("[Console] Cleared all log entries.");
        }
        
        LevelMapper.Map();
        _levels = LoadAssetsOfType<LevelData>();
        Debug.Log($"[SettingsMapper] Mapped {_levels.Length} Level(s) to the Game Settings configuration.");
        _items = LoadAssetsOfType<ItemData>();
        Debug.Log($"[SettingsMapper] Mapped {_items.Length} Item(s) to the Game Settings configuration.");
        _recipes = LoadAssetsOfType<RecipeData>();
        Debug.Log($"[SettingsMapper] Mapped {_recipes.Length} Recipe(s) to the Game Settings configuration.");
        _species = LoadAssetsOfType<SpeciesData>();
        Debug.Log($"[SettingsMapper] Mapped {_species.Length} Species(es) to the Game Settings configuration.");
    }

    public static T[] LoadAssetsOfType<T>() where T : Object
    {
        return AssetDatabaseHelper
            .LoadAssetsOfType<T>()
            .Select(x => x.Value)
            .ToArray();
    }
#endif

    public static ItemData GetItemMatch(ItemData data)
    {
        var name = data?.name ?? throw new ArgumentNullException(nameof(data));
        var item = Data.Items.FirstOrDefault(x => x.name == name) ?? throw new ItemNotFoundException(data);
        return item.Clone();
    }

    public static AudioData GetTrackBySceneIndex(int index) 
        => index switch
        {
            0 => Data.MainMenuMusic,
            1 => Data.InGameMusic,
            _ => null
        };

    #endregion
}
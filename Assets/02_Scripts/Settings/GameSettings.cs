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
    [Header("Cooking")]
    [Tooltip("The amount of time it takes for the noodles to cook.")]
    [SerializeField] private float _noodleBoilingTime = 5.0F;
    [Tooltip("The amount of time it takes for the character to clean the overcooked noodle pot.")]
    [SerializeField] private float _potCleaningTime = 0.2F;
    [Tooltip("The amount of time it takes for the noodles to overcook. The timer starts after the noodles were cooked.")]
    [SerializeField] private float _noodleOvercookTime = 5.0F;
    
    [Header("Customer Behaviour")]
    [Tooltip("The amount of time it takes for the customer to think about his meal.")]
    [SerializeField] private float _customerThinkingTime = 3.0F;
    [Tooltip("The amount of time it takes for the customer to eat.")]
    [SerializeField] private float _customerEatingTime = 3.0F;
    [Tooltip("The amount of time it takes for the customer to stock up the queue, when the one in front is seated.")]
    [SerializeField] private float _queueRestockDelay = 0.2F;
    
    [Header("Assassination")]
    [Tooltip("The amount of time it takes for the customer to die from poison.")]
    [SerializeField] private float _customerDyingTime = 3.0F;
    [Tooltip("The amount of time it takes for the poison cloud to disappear.")]
    [SerializeField] private float _poisonHideDelay = 2.0F;
    [Tooltip("The amount of time between the customers poisoning and his killing animation (Poison Cloud).")]
    [SerializeField] private float _customerKillDelay = 2.0F;
    [Tooltip("The amount of time it takes for the character to clean the table.")]
    [SerializeField] private float _tableCleaningTime = 2.0F;
    
    [Header("Customer Patience")]
    [Tooltip("The amount of time between each tick in the patience system.")]
    [SerializeField] private float _patienceTickDelay = 0.2F;
    [Tooltip("The percentage that drops with each tick from the patience. (0 = Leaving, 100 = Full)")]
    [SerializeField] private float _patienceDropAmount = 2.0F;
    [Tooltip("The amount of time between the angry think bubble and his leave.")]
    [SerializeField] private float _customerAngryLeaveTime = 2.0F;
    
    [Header("Scoring")]
    [Tooltip("The base value each customer gives you when you successfully served them.")]
    [SerializeField] private float _customerBaseScore = 1.0F;
    [Tooltip("The maximum score you could receive from a single customer.")]
    [SerializeField] private float _customerMaxScore = 4.0F;
    [Tooltip("The amount of patience that the customer regains after receiving being seated in percent.")]
    [SerializeField] private float _patienceRegainOnSeated = 20.0F;
    [Tooltip("The amount of patience that the customer regains after receiving an item in percent.")]
    [SerializeField] private float _patienceRegainOnItemReceive = 20.0F;

    [Header("Rendering")] 
    [Tooltip("The poison icon that should be added to an item.")]
    [SerializeField] private SpriteData _poisonIcon;
    
    [Header("References")]
    [Header("Prefabs")]
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private GameObject _customerPrefab;
    [SerializeField] private GameObject _spriteRendererPrefab;
    [SerializeField] private GameObject _rectTransformPrefab;
    [SerializeField] private GameObject _heartsPrefab;

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
    public float TableCleaningTime => _tableCleaningTime;
    public float PatienceTickDelay => _patienceTickDelay;
    public float PatienceDropAmount => _patienceDropAmount;
    public float CustomerAngryLeavingTime => _customerAngryLeaveTime;
    public float CustomerMaxScore => _customerMaxScore;
    public float CustomerBaseScore => _customerBaseScore;
    public float PatienceRegainOnItemReceive => _patienceRegainOnItemReceive;
    public float PatienceRegainOnSeated => _patienceRegainOnSeated;

    public SpriteData PoisonIcon => _poisonIcon;
    
    public GameObject PRE_Item => _itemPrefab;
    public GameObject PRE_SpriteRenderer => _spriteRendererPrefab;
    public GameObject PRE_Customer => _customerPrefab;
    public GameObject PRE_RectTransform => _rectTransformPrefab;
    public GameObject PRE_Hearts => _heartsPrefab;
    
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
    public static GameSettings Data => _data ??= GetSettings();

    internal static GameSettings GetSettings()
    {
#if UNITY_EDITOR
        var settings = AssetDatabase.LoadAssetAtPath<GameSettings>(SETTINGS_PATH);
        if (settings != null) return settings;

        throw new Exception("Could not find the GameSettings!");
#else
        return References.Instance.GetLocalSettings();
#endif
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

        MapCustomers();
        MapGameData();
    }

    [MenuItem("Galaxy Gobbles/Reload Game Settings")]
    public static void ReloadSettings()
    {
        AssetDatabase.ReleaseCachedFileHandles();
        AssetDatabase.Refresh();
        _data = GetSettings();
    }
    
    [MenuItem("Galaxy Gobbles/Map Game Data to Settings")]
    private static void MapGameData()
    {
        if (Data is null)
        {
            Debug.LogWarning("[SettingsMapper] There doesn't seem to be an instance at the moment.");
            return;
        }
        
        Data._levels = LoadAssetsOfType<LevelData>();
        Debug.Log($"[SettingsMapper] Mapped {Data._levels.Length} Level(s) to the Game Settings configuration.");
        Data._items = LoadAssetsOfType<ItemData>();
        Debug.Log($"[SettingsMapper] Mapped {Data._items.Length} Item(s) to the Game Settings configuration.");
        Data._recipes = LoadAssetsOfType<RecipeData>();
        Debug.Log($"[SettingsMapper] Mapped {Data._recipes.Length} Recipe(s) to the Game Settings configuration.");
        Data._species = LoadAssetsOfType<SpeciesData>();
        Debug.Log($"[SettingsMapper] Mapped {Data._species.Length} Species(es) to the Game Settings configuration.");
    }
    
    [MenuItem("Galaxy Gobbles/Map Customers to Levels")]
    private static void MapCustomers()
    {
        LevelMapper.Map();
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
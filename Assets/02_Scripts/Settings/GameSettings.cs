using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// ReSharper disable InconsistentNaming

public class GameSettings : ScriptableObject
{
    [Header("General Settings")]
    [SerializeField] private float _noodleBoilingTime;
    [SerializeField] private float _potCleaningTime;
    [SerializeField] private float _noodleOvercookTime;
    [SerializeField] private float _customerThinkingTime;
    [SerializeField] private float _customerEatingTime;
    [SerializeField] private float _restockCustomerDelay;
    
    [Header("References")]
    [Header("Prefabs")]
    [SerializeField] private GameObject _itemPrefab;
    [SerializeField] private GameObject _customerPrefab;
    [SerializeField] private GameObject _spriteRendererPrefab;

    [Header("Game Data")] 
    [SerializeField] private LevelData[] _levels;
    [SerializeField] private SpeciesData[] _species;
    [SerializeField] private ItemData[] _items;
    [SerializeField] private RecipeData[] _recipes;

    #region Properties
    public float NoodleBoilingTime => _noodleBoilingTime;
    public float PotCleaningTime => _potCleaningTime;
    public float NoodleOvercookTime => _noodleOvercookTime;
    public float CustomerThinkingTime => _customerThinkingTime;
    public float CustomerEatingTime => _customerEatingTime;
    public float RestockCustomerDelay => _restockCustomerDelay;
    
    public GameObject PRE_Item => _itemPrefab;
    public GameObject PRE_SpriteRenderer => _spriteRendererPrefab;
    public GameObject PRE_Customer => _customerPrefab;
    
    public IEnumerable<LevelData> Levels => _levels;
    public IEnumerable<SpeciesData> Species => _species;
    public IEnumerable<ItemData> Items => _items;
    public IEnumerable<RecipeData> Recipes => _recipes;
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
        settings._noodleBoilingTime = 5;
        settings._potCleaningTime = 5;
        settings._noodleOvercookTime = 5;
        settings._customerThinkingTime = 5;
        settings._customerEatingTime = 5;
        settings._restockCustomerDelay = 5;
        return settings;
    }
    #endregion
}
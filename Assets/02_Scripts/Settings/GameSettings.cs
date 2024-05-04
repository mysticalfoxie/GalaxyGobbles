using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

// ReSharper disable InconsistentNaming

public class GameSettings : ScriptableObject
{
    public const string SETTINGS_PATH = "Assets/10_Miscellaneous/03_Settings/CFG_Game Settings.asset";
    
    private static GameSettings _data;
    public static GameSettings Data => _data ??= GetOrCreateSettings();

    [Header("General Settings")]
    [SerializeField] private int _noodleBoilingTime;
    
    [Header("References")]
    [Header("Prefabs")]
    [SerializeField] private GameObject _itemRendererPrefab;
    [SerializeField] private GameObject _customerPrefab;

    [Header("Game Data")] 
    [SerializeField] private LevelData[] _levels;
    [SerializeField] private SpeciesData[] _species;
    [SerializeField] private ItemData[] _items;
    
    #region Properties
    public int NoodleBoilingTime => _noodleBoilingTime;
    public GameObject PRE_ItemRenderer => _itemRendererPrefab;
    public GameObject PRE_Customer => _customerPrefab;
    
    public IEnumerable<LevelData> Levels => _levels;
    public IEnumerable<SpeciesData> Species => _species;
    public IEnumerable<ItemData> Items => _items;
    #endregion
    
    #region Initialization Logic
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
        return settings;
    }
    #endregion
}
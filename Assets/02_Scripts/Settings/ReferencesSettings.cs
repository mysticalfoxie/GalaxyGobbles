using UnityEditor;
using UnityEngine;

// ReSharper disable InconsistentNaming

public class ReferencesSettings : ScriptableObject
{
    public const string SETTINGS_PATH = "Assets/10_Miscellaneous/03_Settings/CFG_References.asset";
    
    private static ReferencesSettings _data;
    public static ReferencesSettings Data => _data ??= GetOrCreateSettings();

    [Header("Prefabs")]
    [SerializeField]
    public GameObject ItemPrefab;
    [SerializeField]
    public GameObject CustomerPrefab;

    internal static ReferencesSettings GetOrCreateSettings()
    {
        var settings = AssetDatabase.LoadAssetAtPath<ReferencesSettings>(SETTINGS_PATH);
        if (settings != null) return settings;
        
        settings = CreateDefaultSettings();
        AssetDatabase.CreateAsset(settings, SETTINGS_PATH);
        AssetDatabase.SaveAssets();
        
        return settings;
    }

    private static ReferencesSettings CreateDefaultSettings()
    {
        var settings = CreateInstance<ReferencesSettings>();
        return settings;
    }
}
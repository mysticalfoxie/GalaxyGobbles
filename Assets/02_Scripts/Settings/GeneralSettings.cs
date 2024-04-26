using UnityEditor;
using UnityEngine;

// ReSharper disable InconsistentNaming

public class GeneralSettings : ScriptableObject, ISettings
{
    public const string SETTINGS_PATH = "Assets/10_Miscellaneous/03_Settings/CFG_General Settings.asset";
    
    private static GeneralSettings _data;
    public static GeneralSettings Data => _data ??= GetOrCreateSettings();
    
    [Header("Cooking")]
    [SerializeField]
    public int NoodleBoilingTime;

    internal static GeneralSettings GetOrCreateSettings()
    {
#if UNITY_EDITOR
        var settings = AssetDatabase.LoadAssetAtPath<GeneralSettings>(SETTINGS_PATH);
        if (settings != null) return settings;
        
        settings = CreateDefaultSettings();
        AssetDatabase.CreateAsset(settings, SETTINGS_PATH);
        AssetDatabase.SaveAssets();
        
        return settings;
#else
        return References.Instance.GeneralSettings;
#endif
    }

    private static GeneralSettings CreateDefaultSettings()
    {
        var settings = CreateInstance<GeneralSettings>();
        settings.NoodleBoilingTime = 5;
        return settings;
    }
}
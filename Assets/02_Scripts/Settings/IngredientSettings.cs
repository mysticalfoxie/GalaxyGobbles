using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

// ReSharper disable InconsistentNaming

public class IngredientSettings : ScriptableObject, ISettings
{
    public const string SETTINGS_PATH = "Assets/10_Miscellaneous/03_Settings/CFG_Item Settings.asset";
    
    private static IngredientSettings _data;
    public static IngredientSettings Data => _data ??= GetOrCreateSettings();

    [Header("List of all Ingredients")]
    [SerializeField]
    public IngredientData[] Ingredients;
    
    internal static IngredientSettings GetOrCreateSettings()
    {
#if UNITY_EDITOR
        var settings = AssetDatabase.LoadAssetAtPath<IngredientSettings>(SETTINGS_PATH);
        if (settings != null) return settings;
        
        settings = CreateDefaultSettings();
        AssetDatabase.CreateAsset(settings, SETTINGS_PATH);
        AssetDatabase.SaveAssets();
        
        return settings;
#else
        return References.Instance.IngredientSettings;
#endif
    }

    private static IngredientSettings CreateDefaultSettings()
    {
        var settings = CreateInstance<IngredientSettings>();
        settings.Ingredients = Array.Empty<IngredientData>();
        return settings;
    }
}
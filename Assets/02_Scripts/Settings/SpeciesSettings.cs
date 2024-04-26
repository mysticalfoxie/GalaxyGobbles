using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

// ReSharper disable InconsistentNaming

public class SpeciesSettings : ScriptableObject
    {
        public const string SETTINGS_PATH = "Assets/10_Miscellaneous/03_Settings/CFG_Species Settings.asset";
        
        [FormerlySerializedAs("Species")]
        [Header("List of all Species")]
        [SerializeField]
        private SpeciesData[] _species;
        
        private static SpeciesSettings _data;
        public static SpeciesSettings Data => _data ??= GetOrCreateSettings();
    
        internal static SpeciesSettings GetOrCreateSettings()
        {
            var settings = AssetDatabase.LoadAssetAtPath<SpeciesSettings>(SETTINGS_PATH);
            if (settings != null) return settings;
        
            settings = CreateDefaultSettings();
            AssetDatabase.CreateAsset(settings, SETTINGS_PATH);
            AssetDatabase.SaveAssets();
        
            return settings;
        }

        private static SpeciesSettings CreateDefaultSettings()
        {
            var settings = CreateInstance<SpeciesSettings>();
            settings._species = Array.Empty<SpeciesData>();
            return settings;
        }
    }
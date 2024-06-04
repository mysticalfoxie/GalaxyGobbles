using System;
using UnityEditor;

public static class SettingsRegister
{
    
#if UNITY_EDITOR
    
    [SettingsProvider]
    public static SettingsProvider CreateGameSettingsProvider() 
        => AssetSettingsProvider
            .CreateProviderFromAssetPath("Galaxy Gobbles/Game Settings", GameSettings.SETTINGS_PATH)
            .EnsureSettingsCreated(() => GameSettings.GetOrCreateSettings())
            .AppendSaveChangesEvent();
    
    [SettingsProvider]
    public static SettingsProvider CreateIdentifierSettingsProvider() 
        => AssetSettingsProvider
            .CreateProviderFromAssetPath("Galaxy Gobbles/Identifiers", Identifiers.SETTINGS_PATH)
            .EnsureSettingsCreated(() => Identifiers.GetOrCreateSettings())
            .AppendSaveChangesEvent();

    #region Extensions
    
    private static AssetSettingsProvider AppendSaveChangesEvent(this AssetSettingsProvider provider)
    {
        provider.inspectorUpdateHandler += () =>
        {
            if (provider.settingsEditor == null) return;
            if (!provider.settingsEditor.serializedObject.UpdateIfRequiredOrScript()) return;
                
            provider.Repaint();
        };

        return provider;
    }

    private static AssetSettingsProvider EnsureSettingsCreated(this AssetSettingsProvider provider, Action action)
    {
        action();
        return provider;
    } 

    #endregion
    
#endif
    
}


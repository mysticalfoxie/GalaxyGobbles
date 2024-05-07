using UnityEditor;

public static class SettingsRegister
{
#if UNITY_EDITOR
    [SettingsProvider]
    public static SettingsProvider CreateReferenceSettingsProvider() 
        => AssetSettingsProvider
            .CreateProviderFromAssetPath("Galaxy Gobbles/Game Settings", GameSettings.SETTINGS_PATH)
            .EnsureSettingsCreated()
            .AppendSaveChangesEvent();

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

    private static AssetSettingsProvider EnsureSettingsCreated(this AssetSettingsProvider provider)
    {
        GameSettings.GetOrCreateSettings();
        return provider;
    } 
#endif
}


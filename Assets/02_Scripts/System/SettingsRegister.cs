using UnityEditor;

public static class SettingsRegister
{
#if UNITY_EDITOR
    [SettingsProvider]
    public static SettingsProvider CreateGeneralSettingsProvider() 
        => AssetSettingsProvider
            .CreateProviderFromAssetPath("Game Design/General", GeneralSettings.SETTINGS_PATH)
            .EnsureSettingsCreated()
            .AppendSaveChangesEvent();
    
    [SettingsProvider]
    public static SettingsProvider CreateReferenceSettingsProvider() 
        => AssetSettingsProvider
            .CreateProviderFromAssetPath("Game Design/References", ReferencesSettings.SETTINGS_PATH)
            .EnsureSettingsCreated()
            .AppendSaveChangesEvent();
    
    [SettingsProvider]
    public static SettingsProvider CreateLevelSettingsProvider() 
        => AssetSettingsProvider
            .CreateProviderFromAssetPath("Game Design/Levels", LevelSettings.SETTINGS_PATH)
            .EnsureSettingsCreated()
            .AppendSaveChangesEvent();
    
    [SettingsProvider]
    public static SettingsProvider CreateSpeciesSettingsProvider() 
        => AssetSettingsProvider
            .CreateProviderFromAssetPath("Game Design/Species", SpeciesSettings.SETTINGS_PATH)
            .EnsureSettingsCreated()
            .AppendSaveChangesEvent();
    
    [SettingsProvider]
    public static SettingsProvider CreateItemSettingsProvider() 
        => AssetSettingsProvider
            .CreateProviderFromAssetPath("Game Design/Items", ItemSettings.SETTINGS_PATH)
            .EnsureSettingsCreated()
            .AppendSaveChangesEvent();
    
    [SettingsProvider]
    public static SettingsProvider CreateIngredientSettingsProvider() 
        => AssetSettingsProvider
            .CreateProviderFromAssetPath("Game Design/Ingredients", IngredientSettings.SETTINGS_PATH)
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
        GeneralSettings.GetOrCreateSettings();
        ReferencesSettings.GetOrCreateSettings();
        LevelSettings.GetOrCreateSettings();
        SpeciesSettings.GetOrCreateSettings();
        ItemSettings.GetOrCreateSettings();
        IngredientSettings.GetOrCreateSettings();
        return provider;
    } 
#endif
}


using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private GameObject _levelButtonPrefab;
    [SerializeField] private GameObject _parentLevelButton;

    private bool _levelLoading;

    private void Awake()
    {
        InitializeButtons();
    }

    public void UpdateLevels()
    {
        var levels = GameSettings.Data.Levels.OrderBy(x => x.Number).ToArray();
        var buttons = GetComponentsInChildren<LevelButton>();
        foreach (var level in levels)
            UpdateButton(buttons, level);
    }

    private static void UpdateButton(IEnumerable<LevelButton> buttons, LevelData level)
    {
        var button = buttons.First(x => x.LevelNumber == level.Number);
        var data = GetLevelSaveData(level);
        button.SetData(data);
    }

    public void InitializeButtons()
    {
        DataManager.Instance.Load();
        var levels = GameSettings.Data.Levels.OrderBy(x => x.Number).ToArray();
        foreach (var level in levels)
            InitializeButton(level);
        DataManager.Instance.SaveChanges();
    }

    private void InitializeButton(LevelData level)
    {
        var instance = Instantiate(_levelButtonPrefab);
        instance.transform!.SetParent(_parentLevelButton.transform, false);
        var button = instance.GetRequiredComponent<LevelButton>();
        var data = GetLevelSaveData(level);
        
        button.Clicked += () => MainMenu.Instance.StartLoadingLevel(level.Number - 1);
        button.SetData(data);
    }

    private static LevelSaveData GetLevelSaveData(LevelData level)
    {
        var levelData = DataManager.GetLevelData().FirstOrDefault(x => x.Number == level.Number);
        if (levelData is null) return CreateLevelSaveData(level);
        return levelData;
    }

    private static LevelSaveData CreateLevelSaveData(LevelData level)
    {
        var data = new LevelSaveData(level);
        DataManager.AddLevelData(data);
        return data;
    }
}
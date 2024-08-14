using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DataManager : Singleton<DataManager>
{
    public const string PREFIX = "savefile-";
    public const string FORMAT = "yyyyMMdd-HHmmss";
    public const string EXTENSION = ".ggs";

    [Header("Debug Mode")] 
    [SerializeField] private bool _enabled;
    
    private bool _changes;

    public GameSaveData Data { get; private set; }

    public DataManager() : base(true) { }

    public void Load()
    {
        var path = GetFilePath();
        if (path is null)
        {
            Log("Could not find save file... Creating new.");
            Instance.Data ??= new GameSaveData();
            SaveInternal();
            return;
        }
        
        var json = File.ReadAllText(path);
        Data = JsonUtility.FromJson<GameSaveData>(json);
        Log($"Loaded save file with {Data?.Levels?.Length ?? 0} level(s).");
    }

    public void SaveChanges()
    {
        Log($"Saving changes...");
        if (!_changes)
        {
            Log("There are no changes.");
            return;
        }
        
        SaveInternal();
        _changes = false;
    }

    public static IEnumerable<LevelSaveData> GetLevelData()
    {
        return Instance.Data?.Levels ?? Array.Empty<LevelSaveData>();
    }

    public static void AddLevelData(LevelSaveData data)
    {
        if (data is null) return;
        Instance.Data ??= new GameSaveData();
        Instance.Data.Levels = Instance.Data.Levels.Append(data).ToArray();
        Instance._changes = true;
        Log($"Added LevelData for level number {data.Number.ToString().PadLeft(2, '0')}.");
    }

    public static void UpdateProgress(int levelNumber, int stars, bool succeeded)
    {
        var level = Instance.Data.Levels.First(x => x.Number == levelNumber);
        if (succeeded && stars > level.Stars) UpdateStars(level, stars);
        if (succeeded) UnlockNextLevel(level);
        Instance.SaveChanges();
    }
    
    private void SaveInternal()
    {
        File.WriteAllText(GetFilePath(true), JsonUtility.ToJson(Data));
        Log($"Saved {Data?.Levels?.Length ?? 0} level(s) to save file.");
    }

    private static string GetFilePath(bool createNew = false)
    {
        var directory = new DirectoryInfo(Application.persistentDataPath);
        if (!directory.Exists) directory.Create();
        var existing = directory.GetFiles().FirstOrDefault(x => x.Name.StartsWith(PREFIX));
        if (existing is not null) return existing.FullName;
        if (!createNew) return null;
        var filename = PREFIX + DateTime.Now.ToString(FORMAT) + EXTENSION;
        return Path.Combine(Application.persistentDataPath, filename);
    }

    private static void Log(string log)
    {
        if (!Debug.isDebugBuild || !Instance._enabled) return;
        Debug.Log($"[Data Manager] {log}");
    }

    private static void UnlockNextLevel(LevelSaveData level)
    {
        var nextLevelIndex = Instance.Data.Levels.IndexOf(level) + 1;
        if (nextLevelIndex > Instance.Data.Levels.Length - 1) return;
        var nextLevel = Instance.Data.Levels[nextLevelIndex];
        if (nextLevel.Unlocked) return;
        nextLevel.Unlocked = true;
        Instance._changes = true;
    }

    private static void UpdateStars(LevelSaveData level, int stars)
    {
        level.Stars = stars;
        Instance._changes = true;
    }
}
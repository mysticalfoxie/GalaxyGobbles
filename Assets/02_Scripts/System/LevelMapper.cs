using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public static class LevelMapper
{
    public static void Map()
    {
#if UNITY_EDITOR
        var customers = AssetDatabaseHelper.LoadAssetsOfType<CustomerData>();
        var levels = AssetDatabaseHelper.LoadAssetsOfType<LevelData>();

        UpdateLevelAssets(levels, customers);
#endif
    }

    private static void UpdateLevelAssets(IEnumerable<(string AssetPath, LevelData Value)> levels, (string AssetPath, CustomerData Value)[] customers)
    {
        foreach (var level in levels)
        {
            var filename = level.AssetPath.Split("/").Last();
            var newLevel = GetUpdatedLevelData(level, customers);
            AssetDatabase.DeleteAsset(level.AssetPath);
            AssetDatabase.CreateAsset(newLevel, level.AssetPath);
            AssetDatabase.SaveAssets();
            
            Debug.Log($"[LevelMapper] Mapped {newLevel._customers.Length} customer(s) to level data file \"{filename}\".");
        }
    }

    private static LevelData GetUpdatedLevelData((string AssetPath, LevelData Value) level, IEnumerable<(string AssetPath, CustomerData Value)> customers)
    {
        var clone = ScriptableObject.CreateInstance<LevelData>();
        clone.name = level.Value.name;
        clone._targetText = level.Value._targetText;
        clone._levelNumber = level.Value._levelNumber;
        clone._closeAfterMinutes = level.Value._closeAfterMinutes;
        clone._closeAfterSeconds = level.Value._closeAfterSeconds;
        clone._customers = GetCustomersForLevel(level, customers);
        return clone;
    }

    private static CustomerData[] GetCustomersForLevel((string AssetPath, LevelData Value) level, IEnumerable<(string AssetPath, CustomerData Value)> customers)
    {
        var levelNumber = GetLevelNumberFromLevelPath(level.AssetPath);
        var levelCustomers = customers
            .Select(GetLevelNumberFromCustomerPath)
            .Where(x => x.LevelNumber == levelNumber)
            .Select(x => x.Customer)
            .ToArray();
        return levelCustomers;
    }

    private static int GetLevelNumberFromLevelPath(string path)
    {
        var filenameRegex = new Regex(@"/([\d\w -()]*\.asset)");
        var filenameMatch = filenameRegex.Match(path);
        var filename = filenameMatch.Groups[1].Value;
        var numberRegex = new Regex(@"(\d+)");
        var numberMatch = numberRegex.Match(filename);
        var value = numberMatch.Groups[1].Value;
        return int.Parse(value);
    }

    private static (int LevelNumber, CustomerData Customer) GetLevelNumberFromCustomerPath((string AssetPath, CustomerData Value) value)
    {
        var path = value.AssetPath;
        var regex = new Regex(@"/(\d+)_Level \d+/");
        var match = regex.Match(path).Groups[1].Value;
        var number = int.Parse(match);
        return (number, value.Value);
    }
}
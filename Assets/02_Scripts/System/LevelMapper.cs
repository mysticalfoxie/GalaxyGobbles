using System.Collections.Generic;
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
#if UNITY_EDITOR
        try
        {
            foreach (var level in levels)
            {
                var filename = level.AssetPath.Split("/").Last();
                var newLevel = GetUpdatedLevelData(level, customers);
                AssetDatabase.DeleteAsset(level.AssetPath);
                AssetDatabase.CreateAsset(newLevel, level.AssetPath);
                AssetDatabase.SaveAssets();

                Debug.Log($"[LevelMapper] Mapped {newLevel.Customers.Count()} customer(s) to level data file \"{filename}\".");
            }
        }
        catch (UnityException ex)
        {
            var isUnavailableException = ex.Message.Contains("Creating asset") && ex.Message.Contains("failed.");
            if (!isUnavailableException) throw;
            Debug.Log("[LevelMapper] The asset database is currently unavailable.");
        }
#endif
    }

    private static LevelData GetUpdatedLevelData((string AssetPath, LevelData Value) level, IEnumerable<(string AssetPath, CustomerData Value)> customers)
    {
        var clone = level.Value.Clone();
        var newCustomers = GetCustomersForLevel(level, customers);
        clone.SetCustomers(newCustomers);
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
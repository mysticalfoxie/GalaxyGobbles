using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = System.Random;

public class CustomerItemRandomizer : MonoBehaviour
{
    [Header("CAREFUL! This overrides ALL MEALS for ALL CUSTOMERS")] [SerializeField]
    private bool _randomizeMeals;

    public void Awake()
    {
        RandomizeMeals();
    }

    private void RandomizeMeals()
    {
#if UNITY_EDITOR
        if (!_randomizeMeals) return;

        var assets = AssetDatabaseHelper.LoadAssetsOfType<CustomerData>();
        var meals = GameSettings.LoadAssetsOfType<ItemData>().Where(x => x.Deliverable).ToArray();
        var random = new Random();

        foreach (var asset in assets)
        {
            var customer = RandomizeItems(random, meals, asset.Value);
            AssetDatabase.DeleteAsset(asset.AssetPath);
            AssetDatabase.CreateAsset(customer, asset.AssetPath);
            AssetDatabase.SaveAssets();
        }
#endif
    }

    private static CustomerData RandomizeItems(Random random, ItemData[] meals, CustomerData customer)
    {
        var chance = random.Next(0, 100);
        if (chance < 30) // 30% chance 
        {
            var item1 = meals[random.Next(0, meals.Length - 1)];
            var item2 = meals[random.Next(0, meals.Length - 1)];
            customer._desiredItems = new[] { item1, item2 };
        }
        else
        {
            var item = meals[random.Next(0, meals.Length - 1)];
            customer._desiredItems = new[] { item };
        }

        return CloneCustomer(customer);
    }

    private static CustomerData CloneCustomer(CustomerData data)
    {
        var clone = ScriptableObject.CreateInstance<CustomerData>();
        clone._desiredItems = data._desiredItems;
        clone._species = data._species;
        clone._isAssassinationTarget = data._isAssassinationTarget;
        clone._minutesInGame = data._minutesInGame;
        clone._secondsInGame = data._secondsInGame;
        clone.name = data.name;
        return clone;
    }
}
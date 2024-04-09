using UnityEngine;

[CreateAssetMenu(fileName = "Meal", menuName = "GameData/Meal", order = 4)]
public class MealData : ScriptableObject
{
    [SerializeField] 
    private ItemType[] _items;
}
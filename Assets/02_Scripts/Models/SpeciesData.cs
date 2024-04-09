using UnityEngine;

[CreateAssetMenu(fileName = "Species", menuName = "GameData/Species", order = 3)]
public class SpeciesData : ScriptableObject
{
    [SerializeField]
    private Sprite[] _sprites;
}
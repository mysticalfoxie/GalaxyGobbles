using UnityEngine;

[CreateAssetMenu(fileName = "Species", menuName = "GameData/Species", order = 3)]
public class SpeciesData : ScriptableObject
{
    [SerializeField]
    private string _name;
    
    [SerializeField]
    private Sprite _sprite;

    public string Name => _name;
    public Sprite Sprite => _sprite;
}
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public const int MAIN_LEVEL_INDEX = 2;
    
    public static int CurrentLevelIndex { get; private set; }
    public static LevelData CurrentLevel { get; private set; }
    public static LevelManager Instance { get; private set; }
    
    [SerializeField]
    // TODO: Validation!! ;)
    private LevelData[] _levels;

    [Header("References")] 
    [SerializeField]
    internal GameObject _customerPrefab;

    private void Awake()
    {
        if (Instance is not null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(Instance);

        var selectedLevel = LevelSelector.selectedLevel - 1;
        LoadLevel(selectedLevel < 0 ? 0 : selectedLevel);
    }

    private void LoadLevel(int index)
    {
        CurrentLevelIndex = index;
        CurrentLevel = _levels
            .OrderBy(x => x.Number)
            .ElementAt(index);
    }
}
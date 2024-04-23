using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static int CurrentLevelIndex { get; private set; }
    public static LevelData CurrentLevel { get; private set; }
    public static LevelManager Instance { get; private set; }
    
    [SerializeField]
    // TODO: Validation!! ;)
    private LevelData[] _levels;

    [Header("References")] 
    [SerializeField]
    internal GameObject _customerPrefab;

    public void Awake()
    {
        if (Instance is not null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(Instance);

        var selectedLevel = LevelSelector.selectedLevel;
        // TODO: Outsource -- Here it's just temporary
        Debug.Log($"Found selectedLevel: {selectedLevel}");
        LoadLevel(selectedLevel-1);
    }

    private void LoadLevel(int index)
    {
        Debug.Log($"Used Index: {index}");
        CurrentLevelIndex = index;
        Debug.Log($"currentLevelIndex: {CurrentLevelIndex}");
        CurrentLevel = _levels
            .OrderBy(x => x.Number)
            .ElementAt(index);
        Debug.Log($"Loaded Level: {CurrentLevel}");
    // + Scene loading needs to happen here~
    }
}
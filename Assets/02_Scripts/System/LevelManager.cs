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
        
        // TODO: Outsource -- Here it's just temporary
        LoadLevel(0);
    }

    private void LoadLevel(int index)
    {
        CurrentLevelIndex = index;
        CurrentLevel = _levels
            .OrderBy(x => x.Number)
            .ElementAt(index);
        // + Scene loading needs to happen here~
    }
}
using System;
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
    

    public void Awake()
    {
        if (Instance is not null)
        {
            Destroy(this);
            return;
        }

        Instance = this;
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
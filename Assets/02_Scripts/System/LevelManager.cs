using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : SingletonMonoBehaviour<LevelManager>
{
    public const int MAIN_LEVEL_INDEX = 1;
    
    [SerializeField]
    // TODO: Validation!! ;)
    private LevelData[] _levels;
    
    public static int CurrentLevelIndex { get; private set; }
    public static LevelData CurrentLevel { get; private set; }
    public IEnumerable<LevelData> Levels => _levels;
    
    public override void Awake()
    {
        base.Awake();
        
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
using System.Linq;

public class LevelManager : SingletonMonoBehaviour<LevelManager>
{
    public const int MAIN_LEVEL_INDEX = 1;
    
    public static int CurrentLevelIndex { get; private set; }
    public static LevelData CurrentLevel { get; private set; }
    
    public override void Awake()
    {
        base.Awake();
        
        var selectedLevel = LevelSelector.selectedLevel - 1;
        LoadLevel(selectedLevel < 0 ? 0 : selectedLevel);
    }

    private static void LoadLevel(int index)
    {
        CurrentLevelIndex = index;
        CurrentLevel = LevelSettings.Data.Levels
            .OrderBy(x => x.Number)
            .ElementAt(index);
    }
}
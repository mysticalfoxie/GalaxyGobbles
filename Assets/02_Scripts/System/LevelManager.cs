using System.Collections;
using System.Linq;
using UnityEngine.SceneManagement;

public class LevelManager : SingletonMonoBehaviour<LevelManager>
{
    public const int MAIN_LEVEL_INDEX = 1;

    public LevelManager() : base(true) { }
    
    public static int CurrentLevelIndex { get; private set; }
    public static LevelData CurrentLevel { get; private set; }
    
    public bool Loading { get; private set; }

    public void LoadLevel(int index)
    {
        CurrentLevelIndex = index;
        CurrentLevel = GameSettings.Data.Levels
            .OrderBy(x => x.Number)
            .ElementAt(index);
        
        MainMenu.Instance.SetElementsForStart();
        StartCoroutine(nameof(LoadScene));
    }

    public IEnumerator LoadScene()
    {
        Loading = true;
        yield return SceneManager.LoadSceneAsync(MAIN_LEVEL_INDEX);
        Loading = false;
    }
}
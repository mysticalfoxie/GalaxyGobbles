using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class KittyBot : Singleton<KittyBot>
{
    public KittyBot()
    {
        InheritedDDoL = true;
    }
    
    protected override void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        this.GetRequiredComponent<Image>().enabled = scene.buildIndex == LevelManager.MAIN_LEVEL_INDEX;
    }
}
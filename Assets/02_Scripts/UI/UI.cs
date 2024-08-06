using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UI : Singleton<UI>
{
    public UI() : base(true) { }

    public override void Awake()
    {
        base.Awake();

        Raycaster = this.GetRequiredComponent<GraphicRaycaster>();
        Canvas = this.GetRequiredComponent<Canvas>();
        if (SceneManager.GetActiveScene().buildIndex == LevelManager.MAIN_LEVEL_INDEX)
            FPSCounter.Show();
    }

    public GraphicRaycaster Raycaster { get; private set; }
    public Canvas Canvas { get; private set; }

    protected override void OnSceneChanged(Scene oldScene, Scene newScene)
    {
        if (newScene.buildIndex != LevelManager.MAIN_LEVEL_INDEX) return;
        FPSCounter.Show();
        ScoreRenderer.Show();
    }

    protected override void OnSceneUnloaded(Scene scene)
    {
        if (scene.buildIndex != LevelManager.MAIN_LEVEL_INDEX) return;
        Hearts.Clear();
        FPSCounter.Hide();
        ScoreRenderer.Hide();
    }
}

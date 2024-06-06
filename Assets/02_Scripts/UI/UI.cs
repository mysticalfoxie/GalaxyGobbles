using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UI : SingletonMonoBehaviour<UI>
{
    public UI() : base(true) { }

    public override void Awake()
    {
        base.Awake();

        Raycaster = this.GetRequiredComponent<GraphicRaycaster>();
        Canvas = this.GetRequiredComponent<Canvas>();
    }

    public GraphicRaycaster Raycaster { get; private set; }
    public Canvas Canvas { get; private set; }
}

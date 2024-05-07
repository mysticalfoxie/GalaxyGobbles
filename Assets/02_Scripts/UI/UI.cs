using UnityEngine.UI;

public class UI : SingletonMonoBehaviour<UI>
{
    public UI() : base(true) { }

    public override void Awake()
    {
        base.Awake();

        Raycaster = this.GetRequiredComponent<GraphicRaycaster>();
    }

    public GraphicRaycaster Raycaster { get; private set; }
}

using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SkipLevel : MonoBehaviour
{
    private Button _button;

#if UNITY_EDITOR
    public void Awake()
    {
        if (!Debug.isDebugBuild)
        {
            Destroy(gameObject);
            return;
        }
        
        _button = this.GetRequiredComponent<Button>();
        _button.onClick.AddListener(OnClick);
    }
#else
    public void Awake()
    {
        Destroy(gameObject);
    }
#endif

    public void OnClick()
    {
        GlobalTimeline.Instance.SkipDay();
    }
}

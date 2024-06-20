using UnityEngine;
using UnityEngine.UI;

public class PatienceToggle : MonoBehaviour
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
        _button.onClick.AddListener(OnClicked);
    }
    #else
    public void Awake()
    {
        Destroy(gameObject);
    }
    #endif

    public static void OnClicked()
    {
        Patience.Disabled = !Patience.Disabled;
        Debug.Log($"[PatienceToggle] Patience System has been {(Patience.Disabled ? "paused" : "resumed")}.");
    } 
}
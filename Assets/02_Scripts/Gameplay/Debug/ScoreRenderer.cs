using TMPro;
using UnityEngine;

public class ScoreRenderer : Singleton<ScoreRenderer>
{
    public const string FORMAT = "0000000";
    
    private TMP_Text _tmpText;
    
    public override void Awake()
    {
        InheritedDDoL = true;
        base.Awake();
        _tmpText = this.GetRequiredComponent<TMP_Text>();
    }

    public void Update() => _tmpText.text = GetValue();
    public void OnValidate()
    {
        _tmpText ??= this.GetRequiredComponent<TMP_Text>();
        _tmpText.text = GetValue();
    }

    private static string GetValue()
    {
        if (BottomBar.Instance is null) return FORMAT;
        if (BottomBar.Instance.Score is null) return FORMAT;
        return BottomBar.Instance.Score.Value.ToString(FORMAT);
    }

    public static void Hide()
    {
        if (!Instance) return;
        Instance.gameObject.SetActive(false);
    }

    public static void Show()
    {
        if (!Instance) return;
        Instance.gameObject.SetActive(true);
    }
}
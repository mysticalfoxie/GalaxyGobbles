using TMPro;
using UnityEngine;

public class ScoreRenderer : MonoBehaviour
{
    public const string FORMAT = "0000000";
    
    private TMP_Text _tmpText;
    
    public void Awake()
    {
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
}
using UnityEngine;
using UnityEngine.UI;

public class OpenStatus : MonoBehaviour
{
    private Text _renderer;
    private bool _isOpen;
    private bool _initialized;

    public void Awake()
    {
        _renderer = this.GetRequiredComponent<Text>();
    }

    public void UpdateTime(int totalSeconds)
    {
        // ReSharper disable once ConvertIfStatementToSwitchStatement
        if (totalSeconds > 0 && !_isOpen)
        {
            _isOpen = true;
            _renderer.text = "OPENED";
        } 
        else if (totalSeconds <= 0 && _isOpen)
        {
            _renderer.text = "CLOSED";
            _isOpen = false;
        }
    }

    public void Reset()
    {
        _renderer.text = string.Empty;
        _isOpen = false;
    }
}
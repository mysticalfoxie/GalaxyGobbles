using UnityEngine;
using UnityEngine.UI;

// ReSharper disable PossibleLossOfFraction

[ExecuteAlways]
public class OpenClosedSign : MonoBehaviour
{
    [Header("Assets")]
    [SerializeField] private Sprite[] _openSigns;
    [SerializeField] private Sprite _closedSign;

    [Header("Debugging")] 
    [SerializeField] private bool _enabled;
    [SerializeField] [Range(0, 100)] private float _current;
    [SerializeField] private float _pps; // percentage per sprite

    public float Current
    {
        get => _current;
        set
        {
            _current = value;
            UpdateSprite();
        }
    }

    private Image _renderer;
    
    public void OnEnable()
    {
        _renderer = this.GetRequiredComponent<Image>();
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        // 54
        // PPS = 5
        // ..., 50, 55, 60, 65, ...
        // Expected: 55
        // 5 * (floor(54/5) + 1)
        // 5 * (floor(10.8) + 1)
        // 5 * (10 + 1)
        // 5 * 11
        // 55

        if (Current <= 0)
        {
            _renderer.sprite = _closedSign;
            return;
        }
        
        _pps = 100F / _openSigns.Length;
        var index = Mathf.FloorToInt(Current / _pps);
        var clamped = Mathf.Max(Mathf.Min(index, _openSigns.Length), 1);
        _renderer.sprite = _openSigns[^clamped];
    }

    private void OnValidate()
    {
        if (!_enabled) return;
        UpdateSprite();
    }

    public void Reset()
    {
        Current = 100;
    }
}

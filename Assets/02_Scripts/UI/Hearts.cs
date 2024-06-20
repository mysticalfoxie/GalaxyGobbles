using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Hearts : MonoBehaviour
{
    private static readonly List<Hearts> _instances = new();
    
    private GameObject _foregroundLayer;
    private GameObject _backgroundLayer;
    private Transform _follow;
    private Vector3 _followPositionO;
    private RectTransform _rectTransform;
    private Image[] _hearts;

    [Header("Debugging")]
    [SerializeField] private bool _debugEnabled;
    [SerializeField] [Range(0.0F, 100.0F)] private int _percentage;
    
    [Header("Visualization")]
    [SerializeField] private Color _heartsUnfilledColor; 
    [SerializeField] private Color _heartsFilledColor;
    
    public Vector2 Offset { get; set; }

    public void Awake()
    {
        _instances.Add(this);
        
        // Fetch layers
        var layer = this.GetChildren().ToArray();
        _backgroundLayer = layer.First();
        _foregroundLayer = layer.Last();

        _hearts = _foregroundLayer.GetComponentsInChildren<Image>();
        _rectTransform = this.GetRequiredComponent<RectTransform>();
        foreach (var image in _backgroundLayer.GetComponentsInChildren<Image>()) image.color = _heartsUnfilledColor;
        foreach (var image in _hearts) image.color = _heartsFilledColor;
    }

    private void Update()
    {
        if (_follow is null) return;
        if (!_follow.IsAssigned()) return;
        if (_follow.position == _followPositionO) return;
        
        _followPositionO = _follow.position;
        var position2D = Raycaster.Instance.Get2DPositionFrom3D(_followPositionO);
        _rectTransform.position = position2D + Offset;
    }

    private void OnValidate()
    {
        if (!_debugEnabled) return;
        Awake();
        SetFillPercentage(_percentage);
    }

    public void SetFillPercentage(float percentage)
    {
        // 100% / 3 Herzen => 33% = 100% für EIN Herz 
        var maxPerHeart = 100.0F / _hearts.Length;
         
        for (var i = 0; i < _hearts.Length; i++)
        {
            var min = maxPerHeart * i;
            var max = min + maxPerHeart;
            var ii = _hearts.Length - i - 1; // inverse index
            
            if (percentage > max)
                _hearts[ii].fillAmount = 1.0F;
            else if (percentage < min)
                _hearts[ii].fillAmount = 0.0F;
            else
                _hearts[ii].fillAmount = CalculateFillAmount(percentage, i, max, maxPerHeart);
        }
    }

    private static float CalculateFillAmount(float percentage, int index, float max, float separator)
    {
        // Percentage:             = 50 %
        // Min:                    = 33 %
        // Max:                    = 66 %
        
        // Index:                  = 1
        // MinValue: 33 - (33 * 1) = 0
        // MaxValue: 66 - (33 * 1) = 33
        // Current:  50 - (33 * 1) = 17

        // 100           = 33
        // 100 / 33      = 1
        // 100 / 33 * 17 = 51.5 (actual percentage)
        // 51.5 / 100    = 0.515 (float percentage)
        
        var maxValue = max - separator * index;
        var current = percentage - separator * index;
        var localPercentage = 100.0F / maxValue * current;
        return localPercentage / 100.0F;
    }

    public void Follow(MonoBehaviour behaviour, Vector2 offset = default)
    {
        _follow = behaviour.gameObject.transform;
        Offset = offset;
    }

    public static void Clear()
    {
        var instances = _instances.Where(instance => instance && instance.gameObject).ToArray();
        foreach (var instance in instances) Destroy(instance.gameObject);
    }
}

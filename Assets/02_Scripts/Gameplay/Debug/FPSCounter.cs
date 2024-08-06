using System.Collections;
using System.Globalization;
using TMPro;
using UnityEngine;

public class FPSCounter : Singleton<FPSCounter>
{
    private float _fps;
    private int _frames;
    private TMP_Text _tmpText;
    
    [SerializeField] private float _capturingDelay = 1.0F;
    [SerializeField] private bool _displayDelta;

#if UNITY_EDITOR
    public override void Awake()
    {
        if (!Debug.isDebugBuild) return;
        base.Awake();
        
        _tmpText = this.GetRequiredComponent<TMP_Text>();
        _tmpText.text = "00";
        if (_displayDelta) _tmpText.text += $" (00.0ms)";
        
        StartCoroutine(nameof(StartSecondTicks));
        Instance.gameObject.SetActive(false);
    }
#else
    public void Awake()
    {
        Destroy(gameObject);
    }
#endif

    public void Update()
    {
        _frames++;
    }

    private IEnumerator StartSecondTicks()
    {
        while (this && enabled)
        {
            yield return new WaitForSecondsRealtime(_capturingDelay);
            
            // 15 / .25 = 60 FPS - When capturing only a quarter of a second
            // 120 / 2  = 60 FPS - When captured only every two seconds 
            _fps = _frames / _capturingDelay;
            _frames = 0; 

            var fpsString = _fps.ToString(CultureInfo.InvariantCulture).PadLeft(3, ' ');
            if (!_displayDelta)
            {
                _tmpText.text = fpsString;
                continue;
            }

            var deltaString = (Time.deltaTime * 1000).ToString("0");
            _tmpText.text = $"{fpsString} ({deltaString}ms)";
        }
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
        Instance.StartCoroutine(nameof(StartSecondTicks));
    }
}
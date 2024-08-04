using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageAnimator : MonoBehaviour
{
    public const int BASE_SPEED_PER_FRAME = 100;
    
    [Header("Animation")]
    [SerializeField] private Sprite[] _sprites;
    [Tooltip("The default speed is 100ms per Frame. Speed of 2 means 50ms. Speed of 0.5 means 200ms. ect.")]
    [Range(0.1F, 10.0F)] [SerializeField] private float _speed = 1.0F;
    [SerializeField] private bool _playOnAwake;
    [SerializeField] private bool _playOnce;
    [SerializeField] private bool _loop;

    private bool _playing;
    private int _index;
    private float _time;
    private Image _image;

    public void OnEnable()
    {
        _image = this.GetRequiredComponent<Image>();
        SceneManager.sceneUnloaded += _ =>
        {
            if (this && isActiveAndEnabled) Destroy(gameObject);
        };
    }

    public void Awake()
    {
        if (_playOnAwake) Play();
    }

    public void Play()
    {
        if (_playing) return;
        
        _index = 0;
        _time = 0F;
        _playing = true;
    }

    public void Update()
    {
        if (!_playing) return;
        _time += Time.unscaledDeltaTime * 1000F;

        var msPerFrame = BASE_SPEED_PER_FRAME / _speed;
        _index = Mathf.FloorToInt(_time / msPerFrame);
        _index = Mathf.Min(Mathf.Max(_index, 0), _sprites.Length - 1);

        if (_time >= msPerFrame * _sprites.Length)
        {
            Stop();
            
            if (_loop) 
                Play();
            else if (_playOnce) 
                Destroy(gameObject);
            return;
        }
        
        _image.sprite = _sprites[_index];
    }

    public void Stop()
    {
        _playing = false;
        _image.sprite = _sprites.Last();
    }
}
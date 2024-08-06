using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Fader : Singleton<Fader>
{
    private static readonly int _fadeSpeedParameter = Animator.StringToHash("FadeSpeed");
    private static readonly int _fadeBlackParameter = Animator.StringToHash("FadeBlack");
    private static readonly int _fadeWhiteParameter = Animator.StringToHash("FadeWhite");
    
    public const string FADE_BLACK_ANIMATION_TAG = "FBS";
    public const string FADE_WHITE_ANIMATION_TAG = "FWS";
    
    private string _currentAnimationTag;
    private bool _animationInProgress;
    private bool _animationDone;
    private Animator _animator;
    private Button[] _buttons;
    private Image _image;

    [Header("Animation Data")]
    [Range(0.01F, 10.0F)] [SerializeField] private float _fadeSpeedMultiplier = 1.0F;
    [SerializeField] private bool _debugging;
    
    public Fader()
    {
        InheritedDDoL = true;
    }

    public override void Awake()
    {
        base.Awake();

        _image = this.GetRequiredComponent<Image>(); 
        _animator = this.GetRequiredComponent<Animator>();
        _buttons = GetComponentsInChildren<Button>();
        _animator.SetFloat(_fadeSpeedParameter, 1.0F);
        _image.enabled = true;
        
        if (!Debug.isDebugBuild && _debugging) 
            _debugging = false;
        
        for (var i = 0; i < _buttons.Length; i++)
        {
            var ci = i;
            _buttons[i].onClick.AddListener(() => { OnDebugButtonClicked(ci); });
            _buttons[i].gameObject.SetActive(_debugging);
        }
    }

    private void OnDebugButtonClicked(int buttonIndex)
    {
        if (!_debugging) return;
        switch (buttonIndex)
        {
            case 0: FadeBlack(); break;
            case 1: FadeWhite(); break;
        }
    }

    public void FadeBlack(float localSpeedModifier = 1.0F)
    {
        if (_animationInProgress) return;
        StartCoroutine(FadeBlackAsync(localSpeedModifier));
    }

    public void FadeWhite(float localSpeedModifier = 1.0F)
    {
        if (_animationInProgress) return;
        StartCoroutine(FadeWhiteAsync(localSpeedModifier));
    }

    public IEnumerator FadeBlackAsync(float localSpeedModifier = 1.0F)
    {
        if (_animationInProgress) yield break;
        if (_debugging) Debug.Log("Fading to black started...");
        _animator.SetFloat(_fadeSpeedParameter, localSpeedModifier * _fadeSpeedMultiplier);
        _animator.SetBool(_fadeBlackParameter, true);
        _animator.SetBool(_fadeWhiteParameter, false);
        _currentAnimationTag = FADE_BLACK_ANIMATION_TAG;
        yield return WaitForAnimationAsync();
        _animator.SetBool(_fadeBlackParameter, false);
        _animator.SetBool(_fadeWhiteParameter, false);
        _animator.SetFloat(_fadeSpeedParameter, _fadeSpeedMultiplier);
        if (_debugging) Debug.Log("Fading to black ended...");
    }

    public IEnumerator FadeWhiteAsync(float localSpeedModifier = 1.0F)
    {
        if (_animationInProgress) yield break;
        if (_debugging) Debug.Log("Fading to white started...");
        _animator.SetFloat(_fadeSpeedParameter, localSpeedModifier * _fadeSpeedMultiplier);
        _animator.SetBool(_fadeBlackParameter, false);
        _animator.SetBool(_fadeWhiteParameter, true);
        _currentAnimationTag = FADE_WHITE_ANIMATION_TAG;
        yield return WaitForAnimationAsync();
        _animator.SetBool(_fadeBlackParameter, false);
        _animator.SetBool(_fadeWhiteParameter, false);
        _animator.SetFloat(_fadeSpeedParameter, _fadeSpeedMultiplier);
        if (_debugging) Debug.Log("Fading to white ended...");
    }

    public IEnumerator FadeBlackWhiteWhile(Action actionsDuringBlack, Action actionAfterComplete = null)
    {
        yield return FadeBlackAsync();
        actionsDuringBlack?.Invoke();
        yield return FadeWhiteAsync();
        actionAfterComplete?.Invoke();
    }
    
    private IEnumerator WaitForAnimationAsync()
    {
        _animationInProgress = true;
        _animationDone = false;
        yield return new WaitWhile(() => !_animationDone);
        _animationInProgress = false;
        _animationDone = false;
    }

    private void ObserveAnimationState()
    {
        if (_currentAnimationTag == default) return;
        if (!_animationInProgress) return;
        
        var state = _animator.GetCurrentAnimatorStateInfo(0);
        if (state.IsTag(_currentAnimationTag)) return; // Still the same tag -> Animation is still playing
        
        _currentAnimationTag = default;
        _animationDone = true;
    }
    
    private void FixedUpdate() => ObserveAnimationState();
}
using System;
using UnityEngine;

[RequireComponent(typeof(TouchExpandAnimator))]
[RequireComponent(typeof(TouchShrinkAnimator))]
public class TouchAnimator : MonoBehaviour
{
    [Header("Touch Animator")]
    [Tooltip("If this animator plays its animation, the linked animators will do so as well.")]
    [SerializeField] private TouchAnimator[] _linkedAnimators = Array.Empty<TouchAnimator>();
    
    private TouchExpandAnimator _expandAnimator;
    private TouchShrinkAnimator _shrinkAnimator;
    private Vector3 _original;

    public void OnEnable()
    {
        _expandAnimator = this.GetRequiredComponent<TouchExpandAnimator>();
        _shrinkAnimator = this.GetRequiredComponent<TouchShrinkAnimator>();
    }

    public void Start()
    {
        _expandAnimator.Original = transform.localScale;
        _shrinkAnimator.Original = transform.localScale;
    }

    public void Shrink()
    {
        foreach (var x in _linkedAnimators) 
            x.ShrinkInternal();
        
        ShrinkInternal();
    }

    public void Expand()
    {
        foreach (var x in _linkedAnimators) 
            x.ExpandInternal();

        ExpandInternal();
    }

    private void ShrinkInternal()
    {
        _expandAnimator.Stop();
        _shrinkAnimator.UpdateConfiguration();
        _shrinkAnimator.Play();
    }

    private void ExpandInternal()
    {
        _shrinkAnimator.Stop();
        _expandAnimator.UpdateConfiguration();
        _expandAnimator.Play();
    }
}
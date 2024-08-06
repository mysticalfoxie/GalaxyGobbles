using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(TouchExpandAnimator))]
[RequireComponent(typeof(TouchShrinkAnimator))]
public class TouchAnimator2D : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [Header("Fallback Handling")]
    [SerializeField] private Vector3 _originalScale = Vector3.one;
    
    private TouchExpandAnimator _expandAnimator;
    private TouchShrinkAnimator _shrinkAnimator;
    private Button _button;

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
        _expandAnimator.Stop();
        _shrinkAnimator.UpdateConfiguration();
        _shrinkAnimator.Play();
    }

    public void Expand()
    {
        _shrinkAnimator.Stop();
        _expandAnimator.UpdateConfiguration();
        _expandAnimator.Play();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Shrink();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Expand();
    }

    private void OnDisable()
    {
        transform.localScale = _originalScale;
    }
}

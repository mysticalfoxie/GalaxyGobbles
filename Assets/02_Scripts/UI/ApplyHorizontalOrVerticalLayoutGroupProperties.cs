using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[RequireComponent(typeof(RectTransform))]
[RequireComponent(typeof(HorizontalOrVerticalLayoutGroup))]
public class ApplyHorizontalOrVerticalLayoutGroupProperties : MonoBehaviour
{
    private HorizontalOrVerticalLayoutGroup _layoutGroup;
    private RectTransform _rectTransform;
    private float _preferredWidth;
    private float _preferredHeight;
    private float _paddingXo;    
    private float _paddingYo;    
    
    [SerializeField] private float _paddingX;
    [SerializeField] private float _paddingY;
    
    private void Update()
    {
        _layoutGroup ??= this.GetRequiredComponent<HorizontalOrVerticalLayoutGroup>();
        _rectTransform ??= this.GetRequiredComponent<RectTransform>();

        if (!_layoutGroup) return;
        if (!_rectTransform) return;
        if (!AnyChanges()) return;
        
        _paddingXo = _paddingX;
        _paddingYo = _paddingY;
        _preferredWidth = _layoutGroup.preferredWidth + _paddingX; 
        _preferredHeight = _layoutGroup.preferredHeight + _paddingY;

        if (_preferredWidth > 0) _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _preferredWidth);
        if (_preferredHeight > 0) _rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _preferredHeight);
    }

    private bool AnyChanges()
    {
        if (!_preferredWidth.Equals(_layoutGroup.preferredWidth)) return true;
        if (!_preferredHeight.Equals(_layoutGroup.preferredHeight)) return true;
        if (!_paddingXo.Equals(_paddingX)) return true;
        if (!_paddingYo.Equals(_paddingY)) return true;
        return false;
    }
}
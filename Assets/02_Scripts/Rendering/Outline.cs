using System;
using UnityEngine;

[ExecuteAlways]
public class Outline : MonoBehaviour
{
    private static readonly int _materialPropertyScale = Shader.PropertyToID("_Scale");
    private static readonly int _materialPropertyColor = Shader.PropertyToID("_Color");
    private static readonly int _materialPropertyEnabled = Shader.PropertyToID("_Enabled");
    
    private Material _material;
    private SpriteRenderer _spriteRenderer;
    private MeshRenderer _meshRenderer;
    private bool _initializing = true;

    private float _scaleO;
    [SerializeField] [Range(0.75F, 1.25F)] private float _scale;
    
    private Color _colorO;
    [SerializeField] private Color _color;
    
    private bool _enabledO;
    [SerializeField] private bool _enabled;

    public void OnEnable()
    {
        _material = new Material(GameSettings.Data.OutlineMaterial);
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _meshRenderer = GetComponent<MeshRenderer>();
    }

    public void FixedUpdate()
    {
        if (_spriteRenderer) UpdateSpriteMaterial();
        if (_meshRenderer) UpdateMeshMaterial();
        
        if (_enabledO != _enabled || _initializing) OnEnabledChange(_enabled);
        if (_colorO != _color || _initializing) OnColorChange(_color);
        if (Math.Abs(_scaleO - _scale) > 0.01 || _initializing) OnThicknessChange(_scale);

        _initializing = false;
    }

    public void Enable() => OnEnabledChange(true);
    public void Disable() => OnEnabledChange(false);
    public void SetColor(Color color) => OnColorChange(color);
    public void SetThickness(float thickness) => OnThicknessChange(thickness);

    private void UpdateMeshMaterial()
    {
        if (!_material) return;
        if (_meshRenderer.sharedMaterial == _material) return;
        _meshRenderer.sharedMaterial = _material;
        if (!_meshRenderer.sharedMaterial) return;
        _meshRenderer.sharedMaterial.SetFloat(_materialPropertyEnabled, _enabled ? 1.0F : 0F);
        _meshRenderer.sharedMaterial.SetFloat(_materialPropertyScale, _scale);
        _meshRenderer.sharedMaterial.SetColor(_materialPropertyColor, _color);
    }

    private void UpdateSpriteMaterial()
    {
        if (!_material) return;
        if (_spriteRenderer.sharedMaterial == _material) return;
        _spriteRenderer.sharedMaterial = _material;
        if (!_spriteRenderer.sharedMaterial) return;
        _spriteRenderer.sharedMaterial.SetFloat(_materialPropertyEnabled, _enabled ? 1.0F : 0F);
        _spriteRenderer.sharedMaterial.SetFloat(_materialPropertyScale, _scale);
        _spriteRenderer.sharedMaterial.SetColor(_materialPropertyColor, _color);
    }

    private void OnEnabledChange(bool value)
    {
        _enabledO = _enabled = value;
        if (_meshRenderer && _meshRenderer.sharedMaterial) _meshRenderer.sharedMaterial.SetFloat(_materialPropertyEnabled, _enabled ? 1.0F : 0F);
        if (_spriteRenderer && _spriteRenderer.sharedMaterial) _spriteRenderer.sharedMaterial.SetFloat(_materialPropertyEnabled, _enabled ? 1.0F : 0F);
    }

    private void OnColorChange(Color value)
    {
        _colorO = _color = value;
        if (_meshRenderer && _meshRenderer.sharedMaterial) _meshRenderer.sharedMaterial.SetColor(_materialPropertyColor, _color);
        if (_spriteRenderer && _spriteRenderer.sharedMaterial) _spriteRenderer.sharedMaterial.SetColor(_materialPropertyColor, _color);
    }

    private void OnThicknessChange(float value)
    {
        _scaleO = _scale = value;
        if (_meshRenderer && _meshRenderer.sharedMaterial) _meshRenderer.sharedMaterial.SetFloat(_materialPropertyScale, _scale);
        if (_spriteRenderer && _spriteRenderer.sharedMaterial) _spriteRenderer.sharedMaterial.SetFloat(_materialPropertyScale, _scale);
    }

    public void OnDestroy()
    {
        if (_material) DestroyImmediate(_material);
        _enabledO = default;
        _colorO = default;
        _scaleO = default;
        _material = default;
    }
}

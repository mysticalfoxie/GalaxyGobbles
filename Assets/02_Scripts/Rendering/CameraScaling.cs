using System;
using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CameraScaling : SingletonMonoBehaviour<CameraScaling>
{
    private Camera _camera;

    [SerializeField] private float _constraint; 

    public float Width { get; set; }
    public float Height { get; set; }
    
    public Vector3 TopLeft { get; private set; }
    public Vector3 TopCenter { get; private set; }
    public Vector3 TopRight { get; private set; }
    public Vector3 MiddleLeft { get; private set; }
    public Vector3 MiddleCenter { get; private set; }
    public Vector3 MiddleRight { get; private set; }
    public Vector3 BottomLeft { get; private set; }
    public Vector3 BottomCenter { get; private set; }
    public Vector3 BottomRight { get; private set; }
    
    public override void Awake()
    {
        base.Awake();
        
        _camera = this.GetRequiredComponent<Camera>();
        _camera.orthographicSize = Calculate();
        CalculateAnchors();
    }

#if UNITY_EDITOR
    public void Update()
    {
        _camera.orthographicSize = Calculate();
        CalculateAnchors();
    }
#endif

    public float Calculate() => 1.0F / _camera.aspect * _constraint / 2.0F;

    public void CalculateAnchors()
    {
        Height = 2.0F * _camera.orthographicSize;
        Width = Height / _camera.aspect;

        var cameraX = _camera.transform.position.x;
        var cameraY = _camera.transform.position.y;

        var leftX = cameraX - Width / 2.0F;
        var rightX = cameraX + Width / 2.0F;
        var topY = cameraY + Height / 2.0F;
        var bottomY = cameraY - Height / 2.0F;

        TopLeft = new Vector3(leftX, topY, 0);
        TopCenter = new Vector3(cameraX, topY, 0);
        TopRight = new Vector3(rightX, topY, 0);
        
        MiddleLeft = new Vector3(leftX, cameraY, 0);
        MiddleCenter = new Vector3(cameraX, cameraY, 0);
        MiddleRight = new Vector3(rightX, cameraY, 0);
        
        BottomLeft = new Vector3(leftX, bottomY, 0);
        BottomCenter = new Vector3(cameraX, bottomY, 0);
        BottomRight = new Vector3(rightX, bottomY, 0);
    }

    public Vector3 GetAnchorPosition(Anchor anchor)
        => anchor switch
        {
            Anchor.TopLeft => TopLeft, 
            Anchor.TopCenter => TopCenter, 
            Anchor.TopRight => TopRight, 
            Anchor.MiddleLeft => MiddleLeft, 
            Anchor.MiddleCenter => MiddleCenter, 
            Anchor.MiddleRight => MiddleRight, 
            Anchor.BottomLeft => BottomLeft, 
            Anchor.BottomCenter => BottomCenter, 
            Anchor.BottomRight => BottomRight,
            _ => throw new ArgumentOutOfRangeException(nameof(anchor), anchor, null)
        };
}
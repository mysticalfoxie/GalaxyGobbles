using UnityEngine;

public class DimensionHelper : SingletonMonoBehaviour<DimensionHelper>
{
    [Header("Raycasting")]
    [SerializeField]
    [Range(1, 100)]
    private float _raycastMaxRange = 10.0F;
    
    public Camera Camera { get; private set; }

    public override void Awake()
    {
        base.Awake();
        Camera = Camera.main;
    }

    public Vector3? Convert2Dto3D(Vector2 vector2)
    {
        var ray = Camera.ScreenPointToRay(vector2);
        Physics.Raycast(ray, out var raycast, _raycastMaxRange);
        
        if (raycast.transform is null) return null;
        if (raycast.collider is null) return null;

        return raycast.point;
    }

    public Vector3? Convert2Dto3D(Vector2 vector2, out GameObject hit)
    {
        hit = null;
        
        var ray = Camera.ScreenPointToRay(vector2);
        Physics.Raycast(ray, out var raycast, _raycastMaxRange);
        
        if (raycast.transform is null) return null;
        if (raycast.collider is null) return null;

        hit = raycast.collider.gameObject;
        return raycast.point;
    }
}
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Raycaster : SingletonMonoBehaviour<Raycaster>
{
    
    [Header("Raycasting")]
    [SerializeField]
    [Range(1, 100)]
    private float _raycastMaxRange = 10.0F;

    public Camera Camera { get; set; }
    
    public override void Awake()
    {
        base.Awake();
        Camera = Camera.main;
    }

    public Vector3? RaycastPosition(Vector2 vector2)
    {
        var ray = Camera.ScreenPointToRay(vector2);
        Physics.Raycast(ray, out var raycast, _raycastMaxRange);
        
        if (raycast.transform is null) return null;
        if (raycast.collider is null) return null;

        return raycast.point;
    }

    public Vector3? Raycast(Vector2 vector2, out GameObject hit)
    {
        hit = null;
        
        var ray = Camera.ScreenPointToRay(vector2);
        Physics.Raycast(ray, out var raycast, _raycastMaxRange);

        if (TryRaycastItems(vector2, out var item))
        { 
            hit = item;
            return vector2;
        }

        if (raycast.transform is null) return null;
        if (raycast.collider is null) return null;

        hit = raycast.collider.gameObject;
        return raycast.point; 
    }

    private static bool TryRaycastItems(Vector2 vector2, out GameObject item)
    {
        var eventArgs = new PointerEventData(EventSystem.current) { position = vector2 };
        var results = new List<RaycastResult>();
        UI.Instance.Raycaster.Raycast(eventArgs, results);
        item = results
            .GroupBy(x => x.gameObject.transform.parent)
            .FirstOrDefault(x => x.Key.CompareTag("Item"))?
            .Key.gameObject;

        if (item is not null) 
            return item;
        
        var match = results.FirstOrDefault(x => x.gameObject.CompareTag("Item")).gameObject;
        if (match is not null) item = match.gameObject;
        
        return item is not null;
    }
}
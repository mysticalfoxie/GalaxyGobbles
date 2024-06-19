using Unity.VisualScripting;
using UnityEngine;

[ExecuteInEditMode]
public class ChairPositionCalculator : MonoBehaviour
{
    [Header("Positions")]
    [SerializeField] private bool _lockPositionBefore;
    [SerializeField] private Vector3 _positionBefore;
    [SerializeField] private Vector3 _positionAfter;
    
    [Header("Result")] 
    [SerializeField] private Vector3 _change;
    
#if UNITY_EDITOR
    public void Update()
    {
        if (!gameObject) return;
        if (!isActiveAndEnabled) return;
        if (!_lockPositionBefore)
        {
            _positionBefore = gameObject.transform.position;
            return;
        }

        _positionAfter = gameObject.transform.position;
        var x = _positionAfter.x - _positionBefore.x;
        var y = _positionAfter.y - _positionBefore.y;
        var z = _positionAfter.z - _positionBefore.z;
        _change = new Vector3(x, y, z);
    }
#endif
}
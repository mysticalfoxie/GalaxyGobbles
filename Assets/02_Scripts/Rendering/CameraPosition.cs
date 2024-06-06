using UnityEngine;

[ExecuteInEditMode]
public class CameraPosition : MonoBehaviour
    {
        [Header("Positioning")]
        [SerializeField] private Anchor _anchor;
        [SerializeField] private Vector3 _offset;

        private void Update()
        {
            if (CameraScaling.Instance is null) return;
            
            var anchorPosition = CameraScaling.Instance.GetAnchorPosition(_anchor);
            var position = anchorPosition + _offset;
            if (position == transform.position) return;
            transform.position = position;
        }
    }
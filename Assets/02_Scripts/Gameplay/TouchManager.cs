using UnityEngine;
using UnityEngine.InputSystem;

public class TouchManager : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    private PlayerInput _playerInput;
    private InputAction _touchPressAction;
    private InputAction _touchPosition;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _touchPressAction = _playerInput.actions["TouchInteract"];
        _touchPosition = _playerInput.actions["TouchPosition"];
    }

    private void OnEnable()
    {
        _touchPressAction.performed += _touchPressed;
    }

    private void OnDisable()
    {
        _touchPressAction.performed -= _touchPressed;
    }

    private void _touchPressed(InputAction.CallbackContext context)
    {

    }
    
}

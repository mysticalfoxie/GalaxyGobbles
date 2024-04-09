using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerMovement : MonoBehaviour
{
    private CustomInput _input = null;
    private Vector3 _moveVector = Vector3.zero;
    private Rigidbody _rigid;
    private float _moveSpeed = 10f;

    private void Awake()
    {
        _input = new CustomInput();
        _rigid = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _rigid.velocity = _moveVector * _moveSpeed;
    }
    private void OnEnable()
    {
        _input.Enable();
        _input.Player.Movement.performed += MovementOnperformed; 
        _input.Player.Movement.canceled += MovementOncanceled; 
    }
    private void OnDisable()
    {
        _input.Disable();
        _input.Player.Movement.performed -= MovementOnperformed; 
        _input.Player.Movement.canceled -= MovementOncanceled; 
    }

    private void MovementOnperformed(InputAction.CallbackContext value)
    {
        _moveVector = value.ReadValue<Vector3>();
    }
    
    private void MovementOncanceled(InputAction.CallbackContext value)
    {
        _moveVector = Vector3.zero;
    }
    


    
    
}

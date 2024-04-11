using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField] private float _currentSpeed = 10.0f;
    private Rigidbody _rb;                        

    void Start()
    {
        _rb = GetComponent<Rigidbody>();  
    }

    // Update is called once per frame
    void Update()
    {
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");
        _rb.MovePosition(_rb.position + new Vector3(horizontal , 0, vertical) * (_currentSpeed * Time.deltaTime));
    }
}

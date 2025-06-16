using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    Rigidbody2D _rigidbody;
    PlayerInput _input;

    Vector2 inputValue;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInput>();
    }


    void FixedUpdate()
    {
        inputValue = _input.actions["Move"].ReadValue<Vector2>();

        _rigidbody.AddForce(inputValue * moveSpeed);
    }
}

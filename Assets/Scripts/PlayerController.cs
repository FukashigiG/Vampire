using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //このスクリプトでは、プレイヤーの操作に関する処理を記述している

    Rigidbody2D _rigidbody;
    PlayerInput _input;
    PlayerStatus _status;

    Vector2 inputValue;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInput>();
        _status = GetComponent<PlayerStatus>();
    }


    void FixedUpdate()
    {
        inputValue = _input.actions["Move"].ReadValue<Vector2>();

        _rigidbody.AddForce(inputValue * _status.moveSpeed);
    }
}

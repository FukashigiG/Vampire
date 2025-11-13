using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : SingletonMono<PlayerController>
{
    //このスクリプトでは、プレイヤーの操作に関する処理を記述している

    Rigidbody2D _rigidbody;
    PlayerInput _input;
    public PlayerStatus _status { get; private set; }

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

    // この関数はPlayerInputによって呼ばれる
    public void OnAbility()
    {
        _status.attack.ExecuteCharaAbility();
    }
}

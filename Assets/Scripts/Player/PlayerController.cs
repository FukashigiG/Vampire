using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;

public class PlayerController : SingletonMono<PlayerController>
{
    //このスクリプトでは、プレイヤーの操作に関する処理を記述している

    [SerializeField] SpriteRenderer _remderer;

    [Header("Turn Settings")]
    [SerializeField, Range(1, 30)]
    private int framesToCompare = 5;

    Rigidbody2D _rigidbody;
    PlayerInput _input;
    public PlayerStatus _status { get; private set; }

    Queue<Vector2> inputQueue = new Queue<Vector2>();

    Vector2 inputValue;

    bool isAlive = true;

    //bool standOfBoost = false;

    public void Initialize(PlayerStatus status)
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _input = GetComponent<PlayerInput>();
        _status = status;

        _remderer.sprite = _status.playerCharaData.image_icon;

        _status.onDie.Subscribe(x =>
        {
            isAlive = false;

            _status.count_Actable++;
            _status.count_PermissionDamage++;
            _status.count_PermissionHit.Value++;

        }).AddTo(this);
    }


    void FixedUpdate()
    {
        if(! isAlive) return;

        inputValue = _input.actions["Move"].ReadValue<Vector2>();

        CheckBoost(inputValue);

        if (inputValue.sqrMagnitude > 0.01)
        {
            

            _rigidbody.AddForce(inputValue * _status.moveSpeed);
        }
        else
        {
            //inputQueue.Clear();
        }

        _rigidbody.AddForce(inputValue * _status.moveSpeed);
    }

    void CheckBoost(Vector2 currentInputDirection)
    {
        // 現在の入力を追加
        inputQueue.Enqueue(currentInputDirection);

        // 各キューと入力を比較
        foreach(var queue in inputQueue)
        {
            // キューがゼロ入力なら、そのキューは無視
            if (queue.sqrMagnitude < 0.01) continue;

            float dotProduct = Vector2.Dot(queue, inputValue.normalized);

            // 各キューと比較して、Nフレーム前の入力と現在の入力ベクトルの内積を計算 (正規化して方向のみを比較)
            // Dot < 0 ならば、角度は90度より大きい
            if (dotProduct <= 0f)
            {
                _rigidbody.linearVelocity = inputValue.normalized;

                _rigidbody.AddForce(inputValue.normalized * _status.moveSpeed * 0.5f, mode: ForceMode2D.Impulse);

                

                // 一度検知したら履歴を一旦クリアする
                //inputQueue.Clear();

                break;
            }
        }

        
        //float dotProduct = Vector2.Dot(inputQueue.Peek(), inputValue.normalized);

        

        // キューが必要数以上溢れないように、古いものを消す
        while (inputQueue.Count > framesToCompare)
        {
            inputQueue.Dequeue();
        }
    }

    // この関数はPlayerInputによって呼ばれる
    public void OnAbility()
    {
        if (!isAlive) return;

        if(GameAdmin.Instance.isPausing) return;

        _status.attack.ExecuteCharaAbility().Forget();
    }
}

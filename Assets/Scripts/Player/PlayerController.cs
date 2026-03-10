using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UniRx;
using System.Linq;

public class PlayerController : SingletonMono<PlayerController>
{
    //このスクリプトでは、プレイヤーの操作に関する処理を記述している

    [SerializeField] SpriteRenderer _remderer;

    [Header("Turn Settings")]
    [SerializeField, Range(1, 30)]
    private int framesToCompare = 5;

    public Rigidbody2D _rigidbody {  get; private set; }
    PlayerInput _input;
    public PlayerStatus _status { get; private set; }

    Queue<Vector2> inputQueue = new Queue<Vector2>();

    List<float> curves = new List<float>();

    public Vector2 inputValue {  get; private set; }

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
        if(! isAlive)
        {
            _rigidbody.linearDamping = 6f;

            return;
        }

        inputValue = _input.actions["Move"].ReadValue<Vector2>();

        //CheckBoost(inputValue.normalized);

        if (inputValue.sqrMagnitude > 0.01)
        {
            _rigidbody.linearDamping = 0f;

            _rigidbody.AddForce((inputValue.normalized * _status.moveSpeed / 10f - _rigidbody.linearVelocity) * 40f);
        }
        else
        {
            //inputQueue.Clear();

            _rigidbody.linearDamping = 6f;
        }

        // 意地でも外に飛び出さないようにする
        var areaSize = GameAdmin.Instance.size_PlayArea;
        var pos = transform.position;
        pos.x = Mathf.Clamp(pos.x, -areaSize, areaSize);
        pos.y = Mathf.Clamp(pos.y, -areaSize, areaSize);
        transform.position =pos;
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

            // 各キューと比較して、Nフレーム前の入力と現在の入力ベクトルの内積を計算 (正規化して方向のみを比較)
            // Dot < 0 ならば、角度は90度より大きい
            float dotProduct = Vector2.Dot(queue, inputValue.normalized);

            float curve = (dotProduct - 1) * -0.5f;

            curves.Add(curve);
        }

        if(curves.Count >= framesToCompare)
        {
            float max = curves.Max();

            max = Mathf.Clamp01(max);

            if (max >= 0.1f)
            {
                _rigidbody.AddForce(currentInputDirection * Mathf.Pow(max, 2f), mode: ForceMode2D.Impulse);
            }

            curves.Clear();
        }

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

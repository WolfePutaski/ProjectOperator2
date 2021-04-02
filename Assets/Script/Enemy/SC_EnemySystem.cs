using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_EnemySystem : MonoBehaviour
{
    public StateMachine _stateMachine;

    private GameObject _PlayerTarget;
    private SC_MoveToObject _MoveToObject;
    private SC_Health _healthSystem;

    [Header("===Speed===")]
    public float DefaultSpeed;
    public float minSpeed;
    public float Acceleration;

    [Header("===Attack===")]
    public float timeToAttack;

    //[SerializeField] SCO_Enemy enemyProfile;

    // Start is called before the first frame update
    void Start()
    {
        _PlayerTarget = GameObject.FindGameObjectWithTag("Player");
        _MoveToObject = gameObject.AddComponent<SC_MoveToObject>();


        TryGetComponent(out _healthSystem);

        var lookDir = GetComponentInChildren<SC_LookDir>();
        lookDir.target = _PlayerTarget.transform;

        _stateMachine = new StateMachine();
        var move = new EnemyState.Move(this, _MoveToObject,_PlayerTarget.transform);
        var attack = new EnemyState.Attack(this);
        var die = new EnemyState.Die(this);

        At(move, attack, closeToTarget());
        AAt(die, noHP());

        _stateMachine.SetState(move);

        void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);
        void AAt(IState to, Func<bool> condition) => _stateMachine.AddAnyTransition(to, condition);

        Func<bool> closeToTarget() => () => Vector2.Distance(_PlayerTarget.transform.position, gameObject.transform.position) <= 1f;
        Func<bool> noHP() => () => _healthSystem.HealthCurrent <= 0f;
    }

    // Update is called once per frame
    void Update()
    {
        _stateMachine.Tick();
    }
}

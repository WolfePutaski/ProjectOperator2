using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_EnemySystem : MonoBehaviour
{
    public StateMachine _stateMachine;

    private GameObject PlayerTarget;
    private SC_Health healthSystem;

    [Header("===Speed===")]
    public float DefaultSpeed;
    public float minSpeed;
    public float Acceleration;

    [Header("===Attack===")]
    public float timeToAttack;

    // Start is called before the first frame update
    void Start()
    {
        PlayerTarget = GameObject.FindGameObjectWithTag("Player");

        TryGetComponent(out healthSystem);

        var lookDir = GetComponentInChildren<SC_LookDir>();
        lookDir.target = PlayerTarget.transform;

        _stateMachine = new StateMachine();
        var move = new EnemyState.Move(this,GetComponent<SC_MoveToObject>(),PlayerTarget.transform);
        var attack = new EnemyState.Attack(this);
        var die = new EnemyState.Die(this);

        At(move, attack, closeToTarget());
        AAt(die, noHP());

        _stateMachine.SetState(move);

        void At(IState to, IState from, Func<bool> condition) => _stateMachine.AddTransition(to, from, condition);
        void AAt(IState to, Func<bool> condition) => _stateMachine.AddAnyTransition(to, condition);

        Func<bool> closeToTarget() => () => Vector2.Distance(PlayerTarget.transform.position, gameObject.transform.position) <= 1f;
        Func<bool> noHP() => () => healthSystem.HealthCurrent <= 0f;
    }

    // Update is called once per frame
    void Update()
    {
        _stateMachine.Tick();
        //Debug.Log(_stateMachine.getCurrentState() != null ? _stateMachine.getCurrentState().GetType().ToString() : "no statemachine") ;
    }
}

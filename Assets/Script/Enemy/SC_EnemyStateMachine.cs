//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class SC_EnemyStateMachine
//{
//    private IState _currentState;
//    private Dictionary<Type, List<Transition>> _transitions = new Dictionary<Type, List<Transition>>;


//    public void Tick()
//    {
//        var transition = GetTransition();
//        if (transition != null)
//            SetState(transition.To);

//        _currentState?.Tick();
//        { }
//    }
//    private class Transition
//    {

//    }

//    public void SetState(IState state)
//    {
//        if (state == _currentState)
//            return;

//        _currentState?.OnExit();
//        _currentState = state;

//        _transitions.TryGetValue(_currentState.GetType(), out _currentTransition)
//    }

//    //public void AddTransition(IState from,IState to,Func<bool> predicate)


//}

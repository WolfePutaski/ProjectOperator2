using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IState
{
    void Tick(float dt);
    void OnEnter();
    void OnExit();

}

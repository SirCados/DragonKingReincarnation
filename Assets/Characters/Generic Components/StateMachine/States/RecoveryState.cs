using UnityEngine;
using System.Collections;

public class RecoveryState : State
{
    float _attackRecoveryDuration;
    float _passedTime = 0;

    public RecoveryState()
    {
    }

    public override void OnEnterState()
    {
        Debug.Log("enter recoveryState");
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnExitState()
    {

    }

}

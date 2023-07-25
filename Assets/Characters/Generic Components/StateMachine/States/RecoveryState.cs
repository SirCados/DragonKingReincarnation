using UnityEngine;

public class RecoveryState : State
{
    float _attackRecoveryDuration;
    float _passedTime = 0;

    public RecoveryState(float attackRecoveryDuration, IdleState idleState)
    {
        _attackRecoveryDuration = attackRecoveryDuration;
        NextState = idleState;
    }

    public override void OnEnterState()
    {

    }

    public override void OnUpdate()
    {
        _passedTime += Time.deltaTime;
        if (_passedTime > _attackRecoveryDuration)
        {
            Debug.Log("should be over");
            ShouldStateChange = true;
        }
    }

    public override void OnExitState()
    {
        ShouldStateChange = false;
    }
}

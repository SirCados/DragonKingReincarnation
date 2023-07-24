using UnityEngine;

public class RecoveryState : State
{
    float _attackRecoveryDuration;
    float _attackRecoveryEnd;

    public RecoveryState(float attackRecoveryDuration)
    {
        _attackRecoveryDuration = attackRecoveryDuration;
    }

    public override void OnEnterState()
    {
        _attackRecoveryEnd = Time.time + _attackRecoveryDuration;
    }

    public override void OnUpdate()
    {
        if(Time.time > _attackRecoveryDuration)
        {
            ShouldStateChange = true;
        }
    }

    public override void OnExitState()
    {
        ShouldStateChange = false;
    }
}

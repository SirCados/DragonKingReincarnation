using UnityEngine;

public class RecoveryState : State
{
    float _attackRecoveryDuration;
    float _attackRecoveryStart;

    public RecoveryState(float attackRecoveryDuration)
    {
        _attackRecoveryDuration = attackRecoveryDuration;
    }

    public override void OnEnterState()
    {
        print("Enter Recovery");
        _attackRecoveryStart = Time.time;
    }

    public override void OnUpdate()
    {
        if(Time.time - _attackRecoveryStart > _attackRecoveryDuration)
        {
            ShouldStateChange = true;
        }
    }

    public override void OnExitState()
    {
        ShouldStateChange = false;
    }
}

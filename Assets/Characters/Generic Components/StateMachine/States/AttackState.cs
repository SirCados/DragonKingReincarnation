using UnityEngine;
using System.Collections;

public class AttackState : State
{
    Hitbox _hitbox;
    float _timeAttackIsActive;
    float _passedTime = 0;


    public AttackState(Hitbox hitbox, float timeAttackIsActive, RecoveryState recoveryState)
    {
        _hitbox = hitbox;
        _timeAttackIsActive = timeAttackIsActive;
        NextState = recoveryState;
    }

    public override void OnEnterState()
    {
        
        _hitbox.gameObject.SetActive(true);
    }

    public override void OnUpdate()
    {
        _passedTime += Time.deltaTime;
        Debug.Log("attackState");
        if (_passedTime >= _timeAttackIsActive)
        {            
            ShouldStateChange = true;
        }
    }

    public override void OnExitState()
    {
        _hitbox.gameObject.SetActive(false);
        ShouldStateChange = false;
        _passedTime = 0;
    }

    IEnumerator AttackTime()
    {
        yield return new WaitForSeconds(_timeAttackIsActive);
    }
}

using UnityEngine;

public class AttackState : State
{
    Hitbox _hitbox;
    float _attackEnds;
    float _timeAttackIsActive;

    public AttackState(Hitbox hitbox, float timeAttackIsActive)
    {
        _hitbox = hitbox;
        _timeAttackIsActive = timeAttackIsActive;
    }

    public override void OnEnterState()
    {

        _hitbox.gameObject.SetActive(true);
        _attackEnds = Time.time + _timeAttackIsActive;
    }

    public override void OnUpdate()
    {
        if (Time.time > _attackEnds)
        {
            ShouldStateChange = true;
        }
    }

    public override void OnExitState()
    {
        _hitbox.gameObject.SetActive(false);
        ShouldStateChange = false;
    }
}

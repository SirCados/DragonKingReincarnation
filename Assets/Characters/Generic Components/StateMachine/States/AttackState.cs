using UnityEngine;

public class AttackState : State
{
    [SerializeField] Hitbox _hitbox;
    float _timeActive;
    float _timeAttackIsActive;
    float _timeCombine;

    public AttackState(Hitbox hitbox, float timeAttackIsActive, State nextState)
    {
        _hitbox = hitbox;
        _timeAttackIsActive = timeAttackIsActive;
        _timeCombine = _timeActive + _timeAttackIsActive;
    }

    public override void OnEnterState()
    {
        print("Enter Attack");

        _hitbox.gameObject.SetActive(true);
        _timeActive = Time.deltaTime;

        print("start at: " + _timeActive + ". End at " + _timeCombine);
    }
    public override void OnUpdate()
    {
        //rename
        
        _timeActive += Time.deltaTime;
        print("time active " + _timeActive);
        if (_timeActive > _timeAttackIsActive)
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

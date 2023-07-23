using UnityEngine;

public class AttackState : State
{
    [SerializeField] Hitbox _hitbox;


    public AttackState(Hitbox hitbox)
    {
        _hitbox = hitbox;
    }

    public override void OnEnterState()
    {
        _hitbox.gameObject.SetActive(true);
    }

    public override void OnExitState()
    {
        _hitbox.gameObject.SetActive(false);
    }
}

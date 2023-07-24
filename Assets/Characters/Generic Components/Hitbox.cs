using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public IAttacker _attacker;

    string _target;
    public bool HasAttackHit = false;  

    private void Awake()
    {
        HitboxSetup();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GiveHitTo(collision);
    }

    private void OnDisable()
    {
        HasAttackHit = false;
    }

    public void GiveHitTo(Collider2D hurtbox)
    {
        if (hurtbox.CompareTag(_target) && !HasAttackHit)
        {
            IHurtbox hurtboxToHit = hurtbox.GetComponent<IHurtbox>();
            hurtboxToHit.TakeHurt(_attacker.GetAttackDamage());
            HasAttackHit = true;
        }
    }
    
    void DetermineTargetType()
    {
        if (gameObject.CompareTag("PlayerHitbox"))
        {
            _target = "EnemyHurtbox";
        }
        else if (gameObject.CompareTag("EnemyHitbox"))
        {
            _target = "PlayerHurtbox";
        }
    }

    void HitboxSetup()
    {
        DetermineTargetType();
        _attacker = GetComponentInParent<IAttacker>();
    }
}

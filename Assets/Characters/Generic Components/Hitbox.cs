using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public IAttacker _attacker;

    [SerializeField] string _willHit;
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
        if (hurtbox.CompareTag(_willHit) && !HasAttackHit)
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
            _willHit = "EnemyHurtbox";
        }
        else if (gameObject.CompareTag("EnemyHitbox"))
        {
            _willHit = "PlayerHurtbox";
        }
    }

    void HitboxSetup()
    {
        DetermineTargetType();
        _attacker = GetComponentInParent<IAttacker>();
    }
}

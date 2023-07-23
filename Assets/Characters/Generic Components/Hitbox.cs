using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public IAttacker _attacker;

    string _target;
    bool _hasAttackHit = false;  

    private void Awake()
    {
        HitboxSetup();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GiveHitTo(collision, _target);
    }

    private void OnDisable()
    {
        _hasAttackHit = false;
    }

    public void GiveHitTo(Collider2D hurtbox, string tag)
    {
        if (hurtbox.CompareTag(tag) && !_hasAttackHit)
        {
            IHurtbox hurtboxToHit = hurtbox.GetComponent<IHurtbox>();
            hurtboxToHit.TakeHurt(_attacker.GetAttackDamage());
            _hasAttackHit = true;
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
        print(_attacker);
    }
}

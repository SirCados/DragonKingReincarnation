using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour, IHitbox
{
    [SerializeField] OrcCharacterController _orc;
    bool _hasAttackHit = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }

    public void GiveHitTo(Collider2D hurtbox, string tag)
    {
        if (hurtbox.CompareTag(tag) && !_hasAttackHit)
        {
            IHurtbox hurtboxToHit = hurtbox.GetComponent<IHurtbox>();
            hurtboxToHit.TakeHurt(_orc.AttackDamage);
            _hasAttackHit = true;
        }        
    }

    public void RotateHitbox()
    {
        throw new System.NotImplementedException();
    }

    public void ResetHitbox()
    {
        if (_hasAttackHit)
        {
            _hasAttackHit = false;
        }
        else
        {
            _hasAttackHit = true;
        }
    }
}

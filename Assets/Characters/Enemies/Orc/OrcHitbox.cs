using UnityEngine;

public class OrcHitbox : MonoBehaviour, IHitbox
{
    [SerializeField] OrcCharacterController _orc;
    bool _hasAttackHit = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GiveHitTo(collision, "PlayerHurtbox");
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
            hurtboxToHit.TakeHurt(_orc.AttackDamage);
            _hasAttackHit = true;
        }
    }
}

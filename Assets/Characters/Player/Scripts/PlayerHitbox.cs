using UnityEngine;

public class PlayerHitbox : MonoBehaviour, IHitbox
{
    [SerializeField] PlayerCharacterController _player;
    bool _hasAttackHit = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GiveHitTo(collision, "EnemyHurtbox");
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
            hurtboxToHit.TakeHurt(_player.AttackDamage);
            _hasAttackHit = true;
        }
    }
}

using UnityEngine;

public class EnemyHurtbox : MonoBehaviour, IDamageable
{    
    public bool IsDead { get; set;}

    [SerializeField] EnemyCharacter _character;
    [SerializeField] GameObject _corpseSprite;

    private void Awake()
    {
        SetupEnemyHurtbox();
    }

    private void LateUpdate()
    {
        CheckIfDead();
    }

    public void TakeDamage(int damageToTake)
    {
        _character.CurrentHealth -= damageToTake;
        print("ow!");

        if (_character.CurrentHealth <= 0)
        {
            _character.CurrentHealth = 0;
            IsDead = true;
        }
    }

    public void CheckIfDead()
    {
        if (IsDead)
        {
            _corpseSprite.SetActive(true);
        }
    }

    void SetupEnemyHurtbox()
    {
        IsDead = false;
        _character = GetComponentInParent<EnemyCharacter>();
    }
}

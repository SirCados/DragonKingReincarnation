using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcCharacterController : MonoBehaviour, IAttacker, IHurtbox
{
    public bool IsDead;
    public float AttackSpeed;
    public float MovementSpeed;
    public int Armor;
    public int AttackDamage;    
    public int MaxHealth;

    [SerializeField] EnemyHitbox _hitbox;

    float _attackStartTime;
    float _attackEndTime;
    float _attackRecoveryTime;
    int _currentHealth; 

    public void OnFire()
    {        
        _hitbox.gameObject.SetActive(true);
    }

    public void AttackRecovery(bool hasAttackHit)
    {
        if (hasAttackHit)
        {
            _hitbox.ResetHitbox();
            _hitbox.gameObject.SetActive(false);
        }
    }

    public void SpawnProjectile(GameObject projectileToSpawn)
    {
       //No projectiles
    }

    public void TakeHurt(int damageToTake)
    {
        if (!IsDead)
        {
            _currentHealth -= damageToTake;
            print("ow!");

            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                IsDead = true;
            }
        }
    }

    void CheckIfDead()
    {
        if (IsDead)
        {
            //be dead
        }
    }

    void SetupOrcCharacterController()
    {
        if (MaxHealth <= 0)
        {
            MaxHealth = 1;
        }
        if (MaxHealth != 0)
        {
            _currentHealth = MaxHealth;
        }
        IsDead = false;
    }

    public int GetAttackDamage()
    {
        throw new System.NotImplementedException();
    }

    public void BeginAttack()
    {
        throw new System.NotImplementedException();
    }
}

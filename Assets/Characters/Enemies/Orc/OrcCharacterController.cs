using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcCharacterController : CharacterStateController, IAttacker
{
    public bool IsDead = false;

    CharacterAttributes _attributes;
    CharacterController _characterController;
    GameObject _hitboxPivot; //used to move hitbox so it faces correct direction
    Hitbox _hitbox;

    //States
    AttackState _attackState;
    RecoveryState _attackRecoveryState;
    IdleState _idleState;
    ChaseState _moveState;
    HurtState _hurtState;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    void SetupOrcCharacterController()
    {
        _attributes = GetComponent<CharacterAttributes>();
        _characterController = GetComponent<CharacterController>();
        _hitboxPivot = GameObject.Find("PlayerHitboxPivot");
        _hitbox = _hitboxPivot.GetComponentInChildren<Hitbox>();
        _attackState = new AttackState(_hitbox, _attributes.AttackSpeed);
        _attackRecoveryState = new RecoveryState(_attributes.AttackSpeed);
        _moveState = new ChaseState();
        _hurtState = new HurtState();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //-- IAttacker functions --//
    public void BeginAttack()
    {
        if (_currentState == _moveState || _currentState == _idleState)
        {
            ChangeState(_attackState, _attackRecoveryState);
        }
    }

    public void SpawnProjectile(GameObject projectileToSpawn)
    {
        //No projectiles to spawn yet.
    }

    public int GetAttackDamage()
    {
        return _attributes.AttackDamage;
    }
    //-- IAttacker functions --//
}

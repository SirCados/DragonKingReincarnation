using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcCharacterController : CharacterStateController, IAttacker, IMobileEnemy
{
    public bool IsDead = false;

    public string StateTracker;

    Animator _animator;
    CharacterAttributes _attributes;
    Hitbox _hitbox;
    TargetDetector _attackRangeDetector;
    TargetDetector _targetDectector;

    //States
    AttackState _attackState;
    RecoveryState _attackRecoveryState;
    IdleState _idleState;
    ChaseState _chaseState;
    HurtState _hurtState;
    WindupState _windupState;

    bool _isAttacking = false;

    void Start()
    {
        SetupOrcCharacterController();
    }

    void SetupOrcCharacterController()
    {
        _animator = GetComponentInChildren<Animator>();
        _attributes = GetComponent<CharacterAttributes>();
        _hitbox = GetComponentInChildren<Hitbox>();
        _hitbox.gameObject.SetActive(false);
        _attackRangeDetector = _hitbox.GetComponentInParent<TargetDetector>();
        _targetDectector = GetComponentInChildren<TargetDetector>();
        _idleState = new IdleState();
        _chaseState = new ChaseState();
        _hurtState = new HurtState();
        _windupState = new WindupState();
        _attackRecoveryState = new RecoveryState();
        _attackState = new AttackState(_hitbox, .1f, _attackRecoveryState);

        ChangeState(_idleState);
    }
    void Update()
    {
        if (_currentState == _idleState || _currentState == _chaseState)
        {
            _isAttacking = false;
        }
        CheckForTarget();
        StateTracker = _currentState.ToString();
    }

    void OrcStateEngine()
    {
        //could I use delegates in constructor of states once all of my states are determined?
        switch (_currentState)
        {
            case IdleState:

                _animator.SetBool("isRecovering", false);
                _animator.SetBool("isWalking", false);
                _animator.SetBool("isIdle", true);
                break;
            case ChaseState:
                _animator.SetBool("isIdle", false);
                _animator.SetBool("isWalking", true);
                MoveToTarget(_targetDectector.TargetPosition);
                break;
            case WindupState:
                Windup();
                break;
            case AttackState:
                Attack();
                break;
            case RecoveryState:
                Recover();
                break;
            case HurtState:
                break;
        }

        StateControllerUpdate();
    }

    void CheckForTarget()
    {
        if (_attackRangeDetector.IsDetecting && !_isAttacking && _currentState == _chaseState)
        {
            print("start windup");
            _isAttacking = true;
            ChangeState(_windupState);
            OrcStateEngine();
        }
        else if (_targetDectector.IsDetecting && !_isAttacking)
        {
            if (_currentState != _chaseState)
            {
                print("start chase");
                ChangeState(_chaseState);
            }
        }
        
        if(_currentState == _chaseState)
        {
            OrcStateEngine();
        }
    }

    public void Windup()
    {
        print("windup begins");

        _animator.SetBool("isWalking", false);
        _animator.SetBool("isWindup", true);
        StartCoroutine(ProcessAttack(_attributes.AttackSpeed/2, _attackState));
    }

    public void Attack()
    {
        print("attack begins");

        _animator.SetBool("isWindup", false);
        _animator.SetBool("isAttacking", true);
        StartCoroutine(ProcessAttack(.15f, _attackRecoveryState));
    }

    public void Recover()
    {
        print("recover begins");
        _animator.SetBool("isAttacking", false);
        _animator.SetBool("isRecovering", true);
        StartCoroutine(ProcessAttack(_attributes.AttackSpeed, _idleState));
    }

    public void BeIdle()
    {
        throw new System.NotImplementedException();
    }

    public int GetAttackDamage()
    {
        return _attributes.AttackDamage;
    }

    public void GoToSleep()
    {
        throw new System.NotImplementedException();
    }

    public void LookForTarget(Vector3 lastPositionOfTarget)
    {
        throw new System.NotImplementedException();
    }

    public void MoveToTarget(Vector3 targetPosition)
    {
        float targetDirectionX = targetPosition.x - transform.position.x;
        float targetDirectionY = targetPosition.y - transform.position.y;
        Vector3 movementVector = new Vector3(targetDirectionX, targetDirectionY, 0).normalized;
        Vector3 movement = movementVector * _attributes.MovementSpeed * Time.deltaTime;

        _animator.SetFloat("xDirection", movementVector.x);
        _animator.SetFloat("yDirection", movementVector.y);
        transform.Translate(movement);
    }

    public void SpawnProjectile(GameObject projectileToSpawn)
    {
        throw new System.NotImplementedException();
    }

    IEnumerator ProcessAttack(float time, State stateToChangeTo)
    {
        yield return new WaitForSeconds(time);
        ChangeState(stateToChangeTo);
        OrcStateEngine();
    }
}
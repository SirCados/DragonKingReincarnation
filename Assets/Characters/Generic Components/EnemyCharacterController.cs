using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacterController : CharacterStateController, IAttacker, IMobileEnemy
{
    public string StateTracker;

    Animator _animator;
    CharacterAttributes _attributes;
    Hitbox _hitbox;
    IHurtbox _hurtbox;
    TargetDetector _attackRangeDetector;
    TargetDetector _targetDectector;

    //States
    AttackState _attackState;
    RecoveryState _attackRecoveryState;
    IdleState _idleState;
    ChaseState _chaseState;
    HurtState _hurtState;
    WindupState _windupState;
    ArmoredState _armoredState;

    bool _isAttacking = false;

    void Start()
    {
        SetupEnemyCharacterController();
    }

    void SetupEnemyCharacterController()
    {
        _animator = GetComponentInChildren<Animator>();
        _attributes = GetComponent<CharacterAttributes>();
        _hitbox = GetComponentInChildren<Hitbox>();
        _hitbox.gameObject.SetActive(false);
        _hurtbox = GetComponent<IHurtbox>();
        _attackRangeDetector = _hitbox.GetComponentInParent<TargetDetector>();
        _targetDectector = GetComponentInChildren<TargetDetector>();
        _idleState = new IdleState();
        _chaseState = new ChaseState();
        _hurtState = new HurtState();
        _windupState = new WindupState();
        _attackRecoveryState = new RecoveryState();
        _attackState = new AttackState(_hitbox, .1f, _attackRecoveryState);
        _armoredState = new ArmoredState();



        ChangeState(_idleState);
    }
    void Update()
    {
        if (_hurtbox.IsDead)
        {
            _hurtbox.ToggleCorpse();
        }
        else
        {
            UpdateEnemyCharacter();
        }
        
    }

    void UpdateEnemyCharacter()
    {
        if (_hurtbox.IsRecoiling)
        {
            _hurtbox.IsRecoiling = false;
            ChangeState(_hurtState);
            CharacterStateEngine();
        }

        if (_hurtbox.IsArmorTooMuch)
        {
            _hurtbox.IsArmorTooMuch = false;
            ChangeState(_armoredState);
            CharacterStateEngine();
        }

        if (_currentState == _idleState || _currentState == _chaseState)
        {
            _isAttacking = false;
        }
        //need to switch to IsInputBlockedExternally || _isInputBlockedInternally, checking this way is untenable
        if (_currentState != _hurtState && _currentState != _armoredState)
        {
            print("looking " + _currentState);
            CheckForTarget();
        }
        StateTracker = _currentState.ToString();
    }

    void CharacterStateEngine()
    {
        //could I use delegates in constructor of states once all of my states are determined?
        //once the game states are established, this can be put in the parent class and called any time state is changed
        switch (_currentState)
        {
            case IdleState:
                BeIdle();
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
                Hurt();
                break;
            case ArmoredState:
                Armored();
                break;
        }

        StateControllerUpdate();
    }

    void CheckForTarget()
    {
        if (_attackRangeDetector.IsDetecting && !_isAttacking && _currentState == _chaseState)
        {
            _isAttacking = true;
            ChangeState(_windupState);
            CharacterStateEngine();
        }
        else if (_targetDectector.IsDetecting && !_isAttacking)
        {
            if (_currentState != _chaseState)
            {
                ChangeState(_chaseState);
            }
        }

        if (_currentState == _chaseState)
        {
            CharacterStateEngine();
        }
    }

    public void BeIdle()
    {

        _animator.SetBool("isHurt", false);
        _animator.SetBool("isRecovering", false);
        _animator.SetBool("isWalking", false);
        _animator.SetBool("isIdle", true);
    }

    public void Windup()
    {
        _animator.SetBool("isWalking", false);
        _animator.SetBool("isWindup", true);
        StartCoroutine(ProcessTimedState(_attributes.AttackSpeed / 2, _attackState));
    }

    public void Attack()
    {
        _animator.SetBool("isWindup", false);
        _animator.SetBool("isAttacking", true);
        StartCoroutine(ProcessTimedState(.15f, _attackRecoveryState));
    }

    public void Recover()
    {
        _animator.SetBool("isAttacking", false);
        _animator.SetBool("isRecovering", true);
        StartCoroutine(ProcessTimedState(_attributes.AttackSpeed, _idleState));
    }

    public void Hurt()
    {
        StopAllCoroutines();
        _animator.SetBool("isIdle", false);
        _animator.SetBool("isWalking", false);
        _animator.SetBool("isWindup", false);
        _animator.SetBool("isAttacking", false);
        _animator.SetBool("isRecovering", false);

        _animator.SetBool("isHurt", true);
        print("damage taken: " + _hurtbox.DamageTaken);
        StartCoroutine(ProcessTimedState((_hurtbox.DamageTaken), _idleState));
    }

    public void Armored()
    {
        StopAllCoroutines();
        _animator.SetBool("isIdle", false);
        _animator.SetBool("isWalking", false);
        _animator.SetBool("isWindup", false);
        _animator.SetBool("isAttacking", false);
        _animator.SetBool("isRecovering", false);

        _animator.SetBool("isHurt", true);
        _animator.SetBool("isHurt", false);
        _animator.SetBool("isIdle", true);
        print("ARMOR!");
        StartCoroutine(ProcessTimedState((.1f), _idleState));
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

    IEnumerator ProcessTimedState(float time, State stateToChangeTo)
    {       
        yield return new WaitForSeconds(time); 
        if(_currentState == _armoredState)
        {
            _hurtbox.ToggleArmorColorOn(false);
        }
        ChangeState(stateToChangeTo);
        CharacterStateEngine();
    }
}

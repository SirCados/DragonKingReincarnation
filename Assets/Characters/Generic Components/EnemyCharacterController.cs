using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCharacterController : CharacterStateController, IAttacker, IMobileEnemy
{
    public string StateTracker;
    public bool IsRabbit = false;
    public bool IsFearless = false;

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
        if (!_hurtbox.IsDead)
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

        if(_attributes.CurrentHealth <= (_attributes.MaxHealth / 2) && !IsFearless)
        {
            IsRabbit = true;
            if(_attributes.MovementSpeed > 6)
            {
                _attributes.MovementSpeed = 6;
            }
        }

        if (_currentState == _idleState || _currentState == _chaseState)
        {
            _isAttacking = false;
        }
        //TODO: need to switch to IsInputBlockedExternally || _isInputBlockedInternally, checking this way is untenable
        if (_currentState != _hurtState && _currentState != _armoredState)
        {
            CheckForTarget();
        }
        StateTracker = _currentState.ToString();
    }

    void CharacterStateEngine()
    {
        //could I use delegates in constructor of states once all of my states are determined?
        //TODO: once the game states are established, this can be put in the parent class and called any time state is changed
        switch (_currentState)
        {
            case IdleState:
                BeIdle();
                break;
            case ChaseState:
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
        } else if (!_targetDectector.IsDetecting)
        {
            ChangeState(_idleState);
            CharacterStateEngine();
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
        StartCoroutine(ProcessTimedState((.5f), _idleState));
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
        _animator.SetBool("isIdle", false);
        _animator.SetBool("isWalking", true);
        float targetDirectionX = targetPosition.x - transform.position.x;
        float targetDirectionY = targetPosition.y - transform.position.y;
        Vector3 directionVector = new Vector3(targetDirectionX, targetDirectionY, 0).normalized;
        if (IsRabbit)
        {
            directionVector = -directionVector;
        }
        Vector3 movement = directionVector * _attributes.MovementSpeed * Time.deltaTime;

        _animator.SetFloat("xDirection", directionVector.x);
        _animator.SetFloat("yDirection", directionVector.y);
        transform.Translate(movement);
    }

    public void SpawnProjectile(GameObject projectileToSpawn)
    {
        throw new System.NotImplementedException();
    }

    IEnumerator ProcessTimedState(float time, State stateToChangeTo)
    {   
        //TODO: Add to state controller class when adding state machine to it
        yield return new WaitForSeconds(time); 
        if(_currentState == _armoredState || _currentState == _hurtState)
        {
            _hurtbox.ToggleHitColorOn(false);
        }
        ChangeState(stateToChangeTo);
        CharacterStateEngine();
    }

    public bool IsSpecial
    {
        get => _isAttacking;
        set => _isAttacking = value;
    }

    public void RecievePower(int points)
    {
        _attributes.PointsOfPower += points;
    }
}

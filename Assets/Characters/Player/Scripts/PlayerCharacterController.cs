using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterController : CharacterStateController, IAttacker
{
    public bool IsDead = false;
    public string StateTracker;

    bool _isAttacking = false;
    Animator _animator;
    CharacterAttributes _attributes;
    Hitbox _hitbox;
    Vector2 _movementVector; //Input from Player Inputs component. Look for OnMove function in this script
    Vector2 _storedMovementVector; //Input from Player Inputs component if not in a moving state. Look for OnMove function in this script

    //States
    AttackState _attackState;
    RecoveryState _attackRecoveryState;
    IdleState _idleState;
    PlayerMoveState _moveState;
    HurtState _hurtState;
    WindupState _windupState;

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
        _idleState = new IdleState();
        _moveState = new PlayerMoveState();
        _hurtState = new HurtState();
        _windupState = new WindupState();
        _attackRecoveryState = new RecoveryState();
        _attackState = new AttackState(_hitbox, .1f, _attackRecoveryState);

        ChangeState(_idleState);
    }
    void Update()
    {
        if (_currentState == _idleState || _currentState == _moveState)
        {
            _isAttacking = false;
            CharacterStateEngine();
        }
        StateTracker = _currentState.ToString();
    }

    void CharacterStateEngine()
    {
        //could I use delegates in constructor of states once all of my states are determined?
        switch (_currentState)
        {
            case IdleState:
                _animator.SetBool("isRecovering", false);
                _animator.SetBool("isWalking", false);
                _animator.SetBool("isIdle", true);
                break;
            case PlayerMoveState:
                _animator.SetBool("isIdle", false);
                _animator.SetBool("isWalking", true);
                MovePlayer();
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

    void OnMove(InputValue movementValue)
    {
        if (_currentState == _hurtState || _isAttacking)
        {
            _movementVector = Vector2.zero;
            _storedMovementVector = movementValue.Get<Vector2>();
        }
        else if (!_isAttacking)
        {
            _storedMovementVector = Vector2.zero;
            _movementVector = movementValue.Get<Vector2>();
            ChangeState(_moveState);
            CharacterStateEngine();
        }
    }

    void MovePlayer()
    {
        if (_storedMovementVector != Vector2.zero)
        {
            _movementVector = _storedMovementVector;
            _storedMovementVector = Vector2.zero;
        }

        if (_movementVector != Vector2.zero)
        {
            Vector3 directionVector = new Vector3(_movementVector.x, _movementVector.y, 0).normalized;
            Vector3 movement = directionVector * _attributes.MovementSpeed * Time.deltaTime;            
            transform.Translate(movement);
        }
        else
        {
            ChangeState(_idleState);
        }
    }

    void OnFire()
    {
        if (!_isAttacking)
        {
            _isAttacking = true;
            _storedMovementVector = _movementVector;
            ChangeState(_windupState);
            CharacterStateEngine();
        }
    }

    public void Windup()
    {
        _animator.SetBool("isWalking", false);
        _animator.SetBool("isWindup", true);
        StartCoroutine(ProcessAttack(_attributes.AttackSpeed / 2, _attackState));
    }

    public void Attack()
    {
        _animator.SetBool("isWindup", false);
        _animator.SetBool("isAttacking", true);
        StartCoroutine(ProcessAttack(.2f, _attackRecoveryState));
    }

    public void Recover()
    {
        _animator.SetBool("isAttacking", false);
        _animator.SetBool("isRecovering", true);
        if (_storedMovementVector != Vector2.zero)
        {
            StartCoroutine(ProcessAttack(_attributes.AttackSpeed, _moveState));
        }
        else
        {
            StartCoroutine(ProcessAttack(_attributes.AttackSpeed, _idleState));
        }
    }

    public int GetAttackDamage()
    {
        return _attributes.AttackDamage;
    }

    public void SpawnProjectile(GameObject projectileToSpawn)
    {
        throw new System.NotImplementedException();
    }

    IEnumerator ProcessAttack(float time, State stateToChangeTo)
    {
        yield return new WaitForSeconds(time);
        ChangeState(stateToChangeTo);
        CharacterStateEngine();
    }
}
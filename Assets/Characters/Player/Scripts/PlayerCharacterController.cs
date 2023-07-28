using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterController : CharacterStateController, IAttacker
{
    public string StateTracker;

    Animator _animator;
    CharacterAttributes _attributes;
    Hitbox _hitbox;
    IHurtbox _hurtbox;
    TargetDetector _attackRangeDetector;
    TargetDetector _targetDectector;
    Vector2 _movementVector; //Input from Player Inputs component. Look for OnMove function in this script
    Vector2 _storedMovementVector; //Input from Player Inputs component if not in a moving state. Look for OnMove function in this script

    //States
    AttackState _attackState;
    RecoveryState _attackRecoveryState;
    IdleState _idleState;
    PlayerMoveState _moveState;
    HurtState _hurtState;
    ClawingState _clawingState;
    BitingState _bitingState;
    ArmoredState _armoredState;

    bool _isAttacking = false;
    bool _isBiting = false;
    public bool IsDead;

    int attackPower = 1;

    void Start()
    {
        SetupPlayerCharacterController();
    }

    void SetupPlayerCharacterController()
    {
        _animator = GetComponentInChildren<Animator>();
        _attributes = GetComponent<CharacterAttributes>();
        _hitbox = GetComponentInChildren<Hitbox>();
        _hitbox.gameObject.SetActive(false);
        _hurtbox = GetComponentInChildren<IHurtbox>();
        _attackRangeDetector = _hitbox.GetComponentInParent<TargetDetector>();
        _targetDectector = GetComponentInChildren<TargetDetector>();
        _idleState = new IdleState();
        _moveState = new PlayerMoveState();
        _hurtState = new HurtState();
        _clawingState = new ClawingState();
        _attackRecoveryState = new RecoveryState();
        _attackState = new AttackState(_hitbox, .1f, _attackRecoveryState);
        _armoredState = new ArmoredState();
        _bitingState = new BitingState();

        print("stats: " + _attributes.MovementSpeed);

        ChangeState(_idleState);
    }
    void Update()
    {
        if (!_hurtbox.IsDead)
        {
            UpdatePlayerCharacter();
            OnBite();
        }
        else
        {
            IsDead = true;
        }
    }

    void UpdatePlayerCharacter()
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

        if (_currentState == _idleState || _currentState == _moveState)
        {
            _isAttacking = false;
            IsSpecial = false;
        }
        if (_currentState == _moveState)
        {
            CharacterStateEngine();
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
            case PlayerMoveState:
                MovePlayer();
                break;
            case ClawingState:
                Claw();
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
            case BitingState:
                Biting();
                break;
        }

        StateControllerUpdate();
    }

    public void GainPower()
    {
        attackPower++;
        _attributes.AttackSpeed -= 0.03f;        
        _attributes.AttackDamage += 1;
        _attributes.MaxHealth += 5;
        _attributes.Armor = _attributes.MaxHealth/20;        
        transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
    }


    public void BeIdle()
    {
        _animator.SetBool("isHurt", false);
        _animator.SetBool("isRecovering", false);
        _animator.SetBool("isWalking", false);
        _animator.SetBool("isIdle", true);
    }

    public void Claw()
    {
        _animator.SetBool("isWalking", false);
        _animator.SetBool("isIdle", false);
        _animator.SetBool("isClawing", true);
        StartCoroutine(ProcessTimedState(_attributes.AttackSpeed / 3, _attackState));
    }

    public void Attack()
    {
        _animator.SetBool("isBiting", false);
        _animator.SetBool("isClawing", false);
        _animator.SetBool("isAttacking", true);
        StartCoroutine(ProcessTimedState(.15f, _attackRecoveryState));
    }

    public void Recover()
    {
        _animator.SetBool("isAttacking", false);
        _animator.SetBool("isRecovering", true);
        if (_storedMovementVector != Vector2.zero)
        {            
            StartCoroutine(ProcessTimedState(_attributes.AttackSpeed, _moveState));
        }
        else
        {
            StartCoroutine(ProcessTimedState(_attributes.AttackSpeed, _idleState));
        }
    }

    public void Hurt()
    {
        StopAllCoroutines();
        _animator.SetBool("isIdle", false);
        _animator.SetBool("isWalking", false);
        _animator.SetBool("isBiting", false);
        _animator.SetBool("isClawing", false);
        _animator.SetBool("isAttacking", false);
        _animator.SetBool("isRecovering", false);

        _animator.SetBool("isHurt", true);
        print("damage taken: " + _hurtbox.DamageTaken);
        StartCoroutine(ProcessTimedState((.5f), _idleState));
    }

    public void Armored()
    {
        StopAllCoroutines();
        _animator.SetBool("isIdle", false);
        _animator.SetBool("isWalking", false);
        _animator.SetBool("isBiting", false);
        _animator.SetBool("isClawing", false);
        _animator.SetBool("isAttacking", false);
        _animator.SetBool("isRecovering", false);

        _animator.SetBool("isHurt", true);
        _animator.SetBool("isHurt", false);
        _animator.SetBool("isIdle", true);
        StartCoroutine(ProcessTimedState((.1f), _idleState));
    }

    public void Biting()
    {
        _animator.SetBool("isWalking", false);
        _animator.SetBool("isIdle", false);
        _animator.SetBool("isBiting", true);
        StartCoroutine(ProcessTimedState(_attributes.AttackSpeed / 2, _attackState));
    }

    public int GetAttackDamage()
    {
        return _attributes.AttackDamage;
    }

    public void LookForTarget(Vector3 lastPositionOfTarget)
    {
        throw new System.NotImplementedException();
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
            _animator.SetBool("isRecovering", false);
            _animator.SetBool("isIdle", false);
            _animator.SetBool("isWalking", true);
            Vector3 directionVector = new Vector3(_movementVector.x, _movementVector.y, 0);

            _animator.SetFloat("xDirection", directionVector.x);
            _animator.SetFloat("yDirection", directionVector.y);
            Vector3 movement = directionVector * _attributes.MovementSpeed * Time.deltaTime;            
            transform.Translate(movement);
        }
        else
        {
            _animator.SetBool("isRecovering", false);
            _animator.SetBool("isWalking", false);
            _animator.SetBool("isIdle", true);
            ChangeState(_idleState);
        }
    }

    void OnFire()
    {
        if (!_isAttacking)
        {
            _isAttacking = true;
            _storedMovementVector = _movementVector;
            ChangeState(_clawingState);
            CharacterStateEngine();
        }
    }

    void OnBite()
    {
        if (Input.GetMouseButton(1) && !_isAttacking)
        {
            _isAttacking = true;
            IsSpecial = true;
            ChangeState(_bitingState);
            CharacterStateEngine();
            print("bite!");
        }
    }

    public void SpawnProjectile(GameObject projectileToSpawn)
    {
        throw new System.NotImplementedException();
    }

    IEnumerator ProcessTimedState(float time, State stateToChangeTo)
    {
        //TODO: Add to state controller class when adding state machine to it
        yield return new WaitForSeconds(time);
        if (_currentState == _armoredState || _currentState == _hurtState)
        {
            _hurtbox.ToggleHitColorOn(false);
        }
        ChangeState(stateToChangeTo);
        CharacterStateEngine();
    }

    public bool IsSpecial
    {
        get => _isBiting;
        set => _isBiting = value;
    }

    public void RecievePower(int points)
    {
        _attributes.PointsOfPower += points;
        GainPower();
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcCharacterController : CharacterStateController, IAttacker, IMobileEnemy
{
    public bool IsDead = false;

    [SerializeField] string StateTracker;

    Animator _animator;
    CharacterAttributes _attributes;
    CharacterController _characterController;    
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
    bool _isCoroutineRunning = false;

    float _passedTime = 0;

    // Start is called before the first frame update
    void Start()
    {
        SetupOrcCharacterController();
    }

    void SetupOrcCharacterController()
    {
        _animator = GetComponentInChildren<Animator>();
        _attributes = GetComponent<CharacterAttributes>();
        _characterController = GetComponent<CharacterController>();        
        _hitbox = GetComponentInChildren<Hitbox>();
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

    // Update is called once per frame
    void Update()
    {
        OrcStateEngine();
        StateTracker = _currentState.ToString();
    }

    void CheckForTarget()
    {
        if (_targetDectector.IsDetecting)
        {
            if (_currentState == _idleState)
            {
                ChangeState(_chaseState);
            }            
        }
        else if(_currentState == _chaseState)
        {
            ChangeState(_idleState);
        }

        if (_currentState == _chaseState && _attackRangeDetector.IsDetecting && !_isAttacking)
        {            
            print("is attacking");
            ChangeState(_windupState);
        }
    }   

    void OrcStateEngine()
    {
        switch (_currentState)
        {
            case AttackState:
                ToggleAnimatorConditions("isAttacking");
                Attack();
                break;
            case ChaseState:
                CheckForTarget();
                ToggleAnimatorConditions("isWalking");
                MoveToTarget(_targetDectector.TargetPosition);
                break;
            case HurtState:
                ToggleAnimatorConditions("isHurt");
                break;
            case IdleState:
                CheckForTarget();
                ToggleAnimatorConditions("isIdle");
                break;
            case RecoveryState:
                ToggleAnimatorConditions("isRecovering");                
                break;
            case WindupState:
                ToggleAnimatorConditions("isWindup");
                AttackWindup();
                break;
        }

        StateControllerUpdate();        
    }

    public void ToggleAnimatorConditions(string conditionToTarget)
    {
        if (_currentState.IsNewState)
        {
            _currentState.IsNewState = true;

            _animator.SetBool("isWalking", false);
            _animator.SetBool("isWindup", false);
            _animator.SetBool("isIdle", false);
            _animator.SetBool("isAttacking", false);
            _animator.SetBool("isRecovering", false);
            _animator.SetBool("isHurt", false);

            switch (conditionToTarget)
            {
                case "isWalking":
                    _animator.SetBool("isWalking", true);
                    break;
                case "isIdle":
                    _animator.SetBool("isIdle", true);
                    break;
                case "isAttacking":
                    _animator.SetBool("isAttacking", true);
                    break;
                case "isRecovering":
                    _animator.SetBool("isRecovering", true);
                    break;
                case "isHurt":
                    _animator.SetBool("isHurt", true);
                    break;
                case "isWindup":
                    _animator.SetBool("isWindup", true);
                    break;
            }
        }
    } 
    
    public void AttackWindup()
    {
        if (!_isAttacking)
        {
            _isAttacking = true;
            print("windup");
            StartCoroutine(AttackProcess());
        }    
    }

    //-- IAttacker functions --//
    public void Attack()
    {
        StartCoroutine(AttackProcess());
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

//-- IMobileEnemy functions --//
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

    public void LookForTarget(Vector3 lastPositionOfTarget)
    {
        //nothing yet
    }

    public void BeIdle()
    {
        //nothing yet
    }

    public void GoToSleep()
    {
        //nothing yet
    }

    IEnumerator AttackProcess()
    {
        if (!_isCoroutineRunning)
        {
            print("wait");
            _isCoroutineRunning = true;
            yield return new WaitForSeconds(_attributes.AttackSpeed);
            print("waiting over");
            if (_currentState == _windupState)
            {
                _isCoroutineRunning = false;
                ChangeState(_attackState);
            }
            else if (_currentState == _attackState)
            {
                print("attack done");
                _isAttacking = false;
                _isCoroutineRunning = false;
                ChangeState(_idleState);
            }
        }        
    }
}

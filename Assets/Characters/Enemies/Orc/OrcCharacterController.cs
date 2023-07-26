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
    GameObject _pivot; //used to move hitbox/target detector/attack range so it faces correct direction
    Hitbox _hitbox;
    TargetDetector _attackRangeDetector;
    TargetDetector _targetDectector;

    //States
    AttackState _attackState;
    RecoveryState _attackRecoveryState;
    IdleState _idleState;
    ChaseState _chaseState;
    HurtState _hurtState;

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
        _pivot = GameObject.Find("Pivot");
        _hitbox = _pivot.GetComponentInChildren<Hitbox>();
        _attackRangeDetector = GameObject.Find("AttackRangeDetector").GetComponent<TargetDetector>();
        _targetDectector = GameObject.Find("TargetDetector").GetComponent<TargetDetector>();

        _idleState = new IdleState();
        _chaseState = new ChaseState();
        _hurtState = new HurtState();
        _attackRecoveryState = new RecoveryState();
        _attackState = new AttackState(_hitbox, 0.1f, _attackRecoveryState);

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
            ChangeState(_chaseState);
        }
        else
        {
            ChangeState(_idleState);
        }
    }

    void CheckIfTargetIsInRange(TargetDetector attackRange)
    {
        if (attackRange.IsDetecting)
        {
            ChangeState(_attackState);           
        }        
    }

    void OrcStateEngine()
    {
        switch (_currentState)
        {
            case AttackState:
                ToggleAnimatorConditions("isAttacking");
                BeginAttack();
                break;
            case ChaseState:
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
                ToggleAnimatorConditions("isIdle");
                HandleAttackSpeed();
                break;
        }

        StateControllerUpdate();        
    }

    public void ToggleAnimatorConditions(string conditionToTarget)
    {
        _animator.SetBool("isWalking", false);
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
        }
    }

    void HandleAttackSpeed()
    {
        _passedTime += Time.deltaTime;
        print("recovery: " + _passedTime);
        if (_passedTime >= 1)
        {
            _passedTime = 0;
            ChangeState(_idleState);
        }                
    }

//-- IAttacker functions --//
    public void BeginAttack()
    {
        //extra stuff can go here
        if (_animator.GetAnimatorTransitionInfo(0).duration == 0 && _currentState == _attackState)
        {
            _currentState = _attackRecoveryState;
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

//-- IMobileEnemy functions --//
    public void MoveToTarget(Vector3 targetPosition)
    {
        float targetDirectionX = targetPosition.x - transform.position.x;
        float targetDirectionY = targetPosition.y - transform.position.y;
        Vector3 movementVector = new Vector3(targetDirectionX, targetDirectionY, 0).normalized;
        Vector3 movement = movementVector * _attributes.MovementSpeed * Time.deltaTime;

        RotatePivot(movementVector);
        _animator.SetFloat("xDirection", movementVector.x);
        _animator.SetFloat("yDirection", movementVector.y);
        print(movementVector);
        transform.Translate(movement);

        CheckIfTargetIsInRange(_attackRangeDetector);
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

    public void RotatePivot(Vector3 movementVector)
    {
        float angle = Mathf.Atan2(movementVector.y, movementVector.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, angle));

        _pivot.transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 10000 * Time.deltaTime);
    }
    //-- IMobileEnemy functions --//
}

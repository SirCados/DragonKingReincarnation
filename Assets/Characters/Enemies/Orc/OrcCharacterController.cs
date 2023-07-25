using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcCharacterController : CharacterStateController, IAttacker, IMobileEnemy
{
    public bool IsDead = false;

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

    // Start is called before the first frame update
    void Start()
    {
        SetupOrcCharacterController();
    }

    void SetupOrcCharacterController()
    {
        //_animator = GetComponent<Animator>();
        _attributes = GetComponent<CharacterAttributes>();
        _characterController = GetComponent<CharacterController>();
        _pivot = GameObject.Find("Pivot");
        _hitbox = _pivot.GetComponentInChildren<Hitbox>();
        _attackRangeDetector = GameObject.Find("AttackRangeDetector").GetComponent<TargetDetector>();
        _targetDectector = GameObject.Find("TargetDetector").GetComponent<TargetDetector>();

        _idleState = new IdleState();
        _chaseState = new ChaseState();
        _hurtState = new HurtState();
        _attackRecoveryState = new RecoveryState(_attributes.AttackSpeed, _idleState);
        _attackState = new AttackState(_hitbox, 0.1f, _attackRecoveryState);

        ChangeState(_idleState);
    }

    // Update is called once per frame
    void Update()
    {
        CheckForTarget();
        OrcStateEngine();
    }

    void CheckForTarget()
    {
        if (_targetDectector.IsDetecting)
        {
            ChangeState(_chaseState);
            CheckIfTargetIsInRange(_attackRangeDetector);
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
            if (_currentState == _chaseState || _currentState == _idleState)
            {
                ChangeState(_attackState);
            }
        }        
    }

    void OrcStateEngine()
    {
        switch (_currentState)
        {
            case AttackState:
                BeginAttack();
                break;
            case ChaseState:
                MoveToTarget(_targetDectector.TargetPosition);
                break;
            case HurtState:
                break;
            case IdleState:
                break;
            case RecoveryState:
                break;
        }

        StateControllerUpdate();
        
    }
    

//-- IAttacker functions --//
    public void BeginAttack()
    {
        //extra stuff can go here
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
        print(movementVector);
        //_characterController.Move(movement);        
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

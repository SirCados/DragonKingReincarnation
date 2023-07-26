using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterController : CharacterStateController, IAttacker
{
    public bool IsInputBlockedExternally = false;
    public bool IsDead = false;
    public float AttackSpeed;
    public float MovementSpeed;
    public int Armor;
    public int AttackDamage;
    public int MaxHealth;

    int _currentHealth;

    AttackState _attackState;
    RecoveryState _attackRecoveryState;
    IdleState _idleState;
    PlayerMoveState _moveState;
    HurtState _hurtState;

    CharacterController _characterController;
    Hitbox _hitbox;
    GameObject _hitboxPivot;

    //bool _isInputBlockedInternally = false;
    Vector2 _movementVector; //Input from Player Inputs component. Look for OnMove function in this script
    Vector2 _storedMovementVector; //Input from Player Inputs component if not in a moving state. Look for OnMove function in this script

    private void Start()
    {
        SetupPlayerCharacterController();
    }
    void SetupPlayerCharacterController()
    {
        if (MaxHealth < 1)
        {
            MaxHealth = 1;
        }
        _currentHealth = MaxHealth;
        _characterController = GetComponent<CharacterController>();
        _hitboxPivot = GameObject.Find("PlayerHitboxPivot");
        _hitbox = _hitboxPivot.GetComponentInChildren<Hitbox>();
        _hitbox.gameObject.SetActive(false);

        _moveState = new PlayerMoveState();
        _idleState = new IdleState();
        _hurtState = new HurtState();
        _attackRecoveryState = new RecoveryState(AttackSpeed, _idleState);
        _attackState = new AttackState(_hitbox, .1f, _attackRecoveryState);

        ChangeState(_idleState);
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
        StateControllerUpdate();
    }

    //From Player Input component. Captures input from specific key bindings
    //To see bindings check "Characters\Player\Player Inputs\DragonKingReincarnation"
    void OnMove(InputValue movementValue)
    {
        if (_currentState != _moveState || _currentState != _idleState)
        {
            _movementVector = Vector2.zero;
            _storedMovementVector = movementValue.Get<Vector2>();
        }
        else
        {
            _storedMovementVector = Vector2.zero;
            _movementVector = movementValue.Get<Vector2>();
        }
    }

    void ProcessInput()
    {
        if (_currentState != _attackState || _currentState == _attackRecoveryState)
        {
            MovePlayer();
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
            ChangeState(_moveState);
            Vector3 movement = new Vector3(_movementVector.x, _movementVector.y, 0);
            _characterController.Move(movement * MovementSpeed * Time.deltaTime);
        }
        else
        {
            ChangeState(_idleState);
        }
    }

    //From Player Input component. Captures input from specific key bindings
    //To see bindings check "Characters\Player\Player Inputs\DragonKingReincarnation"
    void OnFire()
    {
        BeginAttack();

    }

    //-- IAttacker functions --//
    public void BeginAttack()
    {
        if (_currentState == _moveState || _currentState == _idleState)
        {
            ChangeState(_attackState);
        }
    }

    public void SpawnProjectile(GameObject projectileToSpawn)
    {
        //No projectiles to spawn yet.
    }

    public int GetAttackDamage()
    {
        return AttackDamage;
    }
    //-- IAttacker functions --//

    /*
     follow mouse cursor?

    Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);
     */
}

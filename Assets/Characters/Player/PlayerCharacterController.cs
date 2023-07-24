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

    CharacterStateController _stateMachine;

    [SerializeField] AttackState _attackState;
    [SerializeField] RecoveryState _recoveryState;
    //State _currentState;

    CharacterController _characterController;
    Hitbox _hitbox;
    GameObject _hitboxPivot;

    bool _isInputBlockedInternally = false;
    Vector2 _movementVector;
    Vector2 _storedMovementVector;

    private void Start()
    {
        SetupPlayerCharacterController();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
        
    }

    void OnMove(InputValue movementValue)
    {
        if (IsInputBlockedExternally || _isInputBlockedInternally)
        {
            _movementVector = Vector2.zero;
            _storedMovementVector = movementValue.Get<Vector2>();
        }
        else
        {
            _movementVector = movementValue.Get<Vector2>();
        }
    }

    void ProcessInput()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        Vector3 movement = new Vector3(_movementVector.x, _movementVector.y, 0);
        _characterController.Move(movement * MovementSpeed * Time.deltaTime);
    }

    void OnFire()
    {
        print("Attack!");
        BeginAttack();
    }

    public void BeginAttack()
    {
        ChangeState(_attackState, _recoveryState);
    }


    public void SpawnProjectile(GameObject projectileToSpawn)
    {
        //No projectiles to spawn yet.
    }

    void SetupPlayerCharacterController()
    {
        _characterController = GetComponent<CharacterController>();
        _hitboxPivot = GameObject.Find("PlayerHitboxPivot");
        _hitbox = _hitboxPivot.GetComponentInChildren<Hitbox>();
        _hitbox.gameObject.SetActive(false);


        _attackState = new AttackState(_hitbox, .1f);
        print(_attackState);

        _recoveryState = new RecoveryState(AttackSpeed);
        print(_recoveryState);
    }

    public void AttackRecovery(bool hasAttackHit)
    {
        //nothing yet
    }

    public int GetAttackDamage()
    {
        return AttackDamage;
    }

    void AttackUpdate()
    {
        if (_hitbox.HasAttackHit)
        {
            
        }
    }
}

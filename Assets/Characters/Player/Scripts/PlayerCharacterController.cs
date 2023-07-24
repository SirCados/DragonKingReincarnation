using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterController : CharacterStateController, IAttacker, IHurtbox
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
    RecoveryState _recoveryState;
    IdleState _idleState;
    PlayerMoveState _moveState;
    HurtState _hurtState;

    CharacterController _characterController;
    Hitbox _hitbox;
    GameObject _hitboxPivot;

    bool _isInputBlockedInternally = false;
    Vector2 _movementVector; //Input from Player Inputs component. Look for OnMove function in this script
    Vector2 _storedMovementVector; //Input from Player Inputs component if not in a moving state. Look for OnMove function in this script

    private void Start()
    {
        SetupPlayerCharacterController();
    }
    void SetupPlayerCharacterController()
    {
        if(MaxHealth < 1)
        {
            MaxHealth = 1;
        }
        _currentHealth = MaxHealth;
        _characterController = GetComponent<CharacterController>();
        _hitboxPivot = GameObject.Find("PlayerHitboxPivot");
        _hitbox = _hitboxPivot.GetComponentInChildren<Hitbox>();
        _hitbox.gameObject.SetActive(false);

        _attackState = new AttackState(_hitbox, .1f);
        _recoveryState = new RecoveryState(AttackSpeed);
        _moveState = new PlayerMoveState();
        _idleState = new IdleState();
        _hurtState = new HurtState();
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
        MovePlayer();
    }

    void MovePlayer()
    {
        if(_storedMovementVector != Vector2.zero)
        {
            _movementVector = _storedMovementVector;
            _storedMovementVector = Vector2.zero;
        }

        if(_movementVector != Vector2.zero)
        {
            ChangeState(_moveState);
            Vector3 movement = new Vector3(_movementVector.x, _movementVector.y, 0);
            _characterController.Move(movement * MovementSpeed * Time.deltaTime);
        }
        else
        {
            _currentState = _idleState;
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
            ChangeState(_attackState, _recoveryState);
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

 //-- IHurtbox function --//
    public void TakeHurt(int damageToTake)
    {
        if (!IsDead)
        {
            _currentHealth -= damageToTake;
            print("ow!");

            if (_currentHealth <= 0)
            {
                _currentHealth = 0;
                IsDead = true;
            }
        }
    }
//-- IHurtbox function --//        
}

using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterController : CharacterStateMachine, IAttacker
{
    public bool IsInputBlockedExternally = false;
    public bool IsDead = false;
    public float AttackSpeed;
    public float MovementSpeed;
    public int Armor;
    public int AttackDamage;
    public int MaxHealth;

    [SerializeField] CharacterController _characterController;
    [SerializeField] PlayerHitbox _hitbox;
    [SerializeField] GameObject _hitboxPivot;

    //AttackState _attackState = new AttackState(_hitbox);

    bool _isInputBlockedInternally = false;
    Vector2 _movementVector;
    Vector2 _storedMovementVector;

    float _movementSpeed = 20.0f;

    private void Awake()
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
        _characterController.Move(movement * _movementSpeed * Time.deltaTime);
    }

    void OnFire()
    {
        print("Attack!");
        BeginAttack();
    }

    public void BeginAttack()
    {
        //ChangeState();
    }
   

    public void SpawnProjectile(GameObject projectileToSpawn)
    {
        //No projectiles to spawn yet.
    }

    void SetupPlayerCharacterController()
    {
        _characterController = GetComponent<CharacterController>();
        _hitboxPivot = GameObject.Find("PlayerHitboxPivot");
        _hitbox = _hitboxPivot.GetComponentInChildren<PlayerHitbox>();
        _hitbox.gameObject.SetActive(false);
    }

    public void AttackRecovery(bool hasAttackHit)
    {
        //nothing yet
    }

    public int GetAttackDamage()
    {
        return AttackDamage;
    }
}

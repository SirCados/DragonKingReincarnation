using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControllerScript : MonoBehaviour
{
    public bool IsInputBlockedExternally = false;
    public bool IsGameOver = false;

    bool _isInputBlockedInternally = false;
    CharacterController _characterController;
    Vector2 _movementVector;
    Vector2 _storedMovementVector;

    float _movementSpeed = 20.0f;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    void OnMove(InputValue movementValue)
    {
        if(IsInputBlockedExternally || _isInputBlockedInternally)
        {
            _movementVector = Vector2.zero;
            _storedMovementVector = movementValue.Get<Vector2>();
        }
        else
        {
            _movementVector = movementValue.Get<Vector2>();
        }
    }

    void MovePlayer()
    {
        Vector3 movement = new Vector3(_movementVector.x, _movementVector.y, 0);
        _characterController.Move(movement * _movementSpeed * Time.deltaTime);
    }
}

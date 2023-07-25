using UnityEngine;

public class TargetDetector : MonoBehaviour
{
    public Vector3 TargetPosition = Vector3.zero;
    public TargetType WillAttack = TargetType.PLAYER;
    public bool IsDetecting;

    string _tagToLookFor;

    public enum TargetType
    {
        PLAYER,
        ENEMY
    }

    private void Awake()
    {
        DetermineTargetType();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerHurtbox"))
        {
            print("collision!");
            TargetPosition = collision.transform.position;
            IsDetecting = true;
        } 
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerHurtbox"))
        {
            TargetPosition = collision.transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("PlayerHurtbox"))
        {
            IsDetecting = false;
        }
    }

    void DetermineTargetType()
    {
        switch (WillAttack)
        {
            case TargetType.PLAYER:
                _tagToLookFor = "PlayerHurtbox";
                break;
            case TargetType.ENEMY:
                _tagToLookFor = "EnemyHurtbox";
                break;
        }
    }
}

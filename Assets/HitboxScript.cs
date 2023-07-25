using UnityEngine;

public class HitboxScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("EnemyHurtbox") || other.CompareTag("PlayerHurtbox"))
        {

        }
    }
}

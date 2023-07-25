using UnityEngine;

public class CharacterAttributes : MonoBehaviour
{
    public float AttackSpeed;
    public float MovementSpeed;
    public int Armor;
    public int AttackDamage;
    public int CurrentHealth;
    public int MaxHealth;

    // Start is called before the first frame update
    void Start()
    {
        SetupEnemyCharacter();
    }

    void SetupEnemyCharacter()
    {
        if (MaxHealth <= 0)
        {
            MaxHealth = 1;
        }
        if (MaxHealth != 0)
        {
            CurrentHealth = MaxHealth;
        }
    }
}

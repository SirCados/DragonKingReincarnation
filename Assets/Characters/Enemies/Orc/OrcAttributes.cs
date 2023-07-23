using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrcAttributes
{
    
    public float AttackSpeed { get; set; }
    public float MovementSpeed { get; set; }
    public int Armor { get; set; }
    public int AttackDamage { get; set; }
    public int CurrentHealth { get; set; }
    public int MaxHealth { get; set; }

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

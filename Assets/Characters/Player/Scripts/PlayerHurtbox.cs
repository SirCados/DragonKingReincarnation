using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtbox : MonoBehaviour, IHurtbox
{
    public bool IsDead;

    [SerializeField] CharacterAttributes _character;
    [SerializeField] GameObject _corpseSprite;

    private void Awake()
    {
        SetupEnemyHurtbox();
    }

    private void LateUpdate()
    {
        CheckIfDead();
    }

    public void CheckIfDead()
    {
        if (IsDead && _corpseSprite != null)
        {
            _corpseSprite.SetActive(true);
        }
    }

    void SetupEnemyHurtbox()
    {
        IsDead = false;
        _character = GetComponentInParent<CharacterAttributes>();
    }

    public void TakeHurt(int damageToTake)
    {
        _character.CurrentHealth -= damageToTake;
        print("ow!");

        if (_character.CurrentHealth <= 0)
        {
            print("Ded x_x");
            _character.CurrentHealth = 0;
            IsDead = true;
        }
    }
}

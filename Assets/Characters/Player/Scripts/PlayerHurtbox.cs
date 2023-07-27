using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurtbox : MonoBehaviour, IHurtbox
{
    bool _isOutOfHealth;

    bool _isRecoiling = false;
    int _damageTaken;
    CharacterAttributes _character;
    [SerializeField] GameObject _corpseSprite;

    private void Awake()
    {
        SetupEnemyHurtbox();
    }

    public void ToggleCorpse()
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

    public void ToggleArmorColorOn(bool isArmorTooMuch)
    {
        throw new System.NotImplementedException();
    }

    public bool IsRecoiling
    {
        get => _isRecoiling;
        set => _isRecoiling = value;
    }
    public bool IsDead
    {
        get => _isOutOfHealth;
        set => _isOutOfHealth = value;
    }

    public int DamageTaken
    {
        get => _damageTaken;
    }
    public bool IsArmorTooMuch { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
}

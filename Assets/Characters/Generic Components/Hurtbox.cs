using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hurtbox : MonoBehaviour, IHurtbox
{
    bool _isOutOfHealth;

    bool _isCorpse;
    bool _isEaten;

    bool _isRecoiling = false;
    bool _isArmorTooMuch = false;
    int _damageToTake; 
    
    CharacterAttributes _attributes;

    [SerializeField] GameObject _corpseSprite;
    [SerializeField] GameObject _sprite;
    [SerializeField] GameObject _eatenSprite;
    public int PointValue;

    SpriteRenderer _spriteRenderer;
    Color _defaultColor;


    private void Awake()
    {
        SetupEnemyHurtbox();
    }

    public void ToggleCorpse()
    {
        IsDead = true;
        _isCorpse = true;
        _sprite.SetActive(false);
        _corpseSprite.SetActive(true);
    }

    public void ToggleEaten()
    {
        _isEaten = true;
        _corpseSprite.SetActive(false);
        _eatenSprite.SetActive(true);        
    }

    void SetupEnemyHurtbox()
    {
        IsDead = false;
        IsArmorTooMuch = false;
        _sprite.SetActive(true);
        _spriteRenderer = _sprite.GetComponent<SpriteRenderer>();
        _defaultColor = _spriteRenderer.color;
        _corpseSprite.SetActive(false);
        _eatenSprite.SetActive(false);
        _attributes = GetComponentInParent<CharacterAttributes>();        
    }

    public void TakeHurt(int incomingDamage, bool isSpecial)
    {
        if (!_isEaten && !_isCorpse)
        {
            _damageToTake = incomingDamage - _attributes.Armor;
            if (_damageToTake < 1)
            {
                IsArmorTooMuch = true;

                ToggleHitColorOn(true);
                _damageToTake = 0;
            }
            else
            {
                IsRecoiling = true;
                _attributes.CurrentHealth -= _damageToTake;
                ToggleHitColorOn(true);

                if (_attributes.CurrentHealth <= 0)
                {
                    _attributes.CurrentHealth = 0;
                    ToggleCorpse();
                }
            }
        }
        else if(isSpecial)
        {
            ToggleEaten();
        }
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

    public bool IsArmorTooMuch
    {
        get => _isArmorTooMuch;
        set => _isArmorTooMuch = value;
    }

    public bool IsCorpse
    {
        get => _isCorpse;
    }

    public bool IsEaten
    {
        get => _isEaten;
    }

    public int DamageTaken
    {
        get => _damageToTake;
    }

    public int GivePoints
    {
        get => _attributes.PointsOfPower;
    }

    public void ToggleHitColorOn(bool isInHitState)
    {
        if (isInHitState)
        {
            _spriteRenderer.color = (IsArmorTooMuch) ? Color.grey : Color.red;
        }
        else
        {
            _spriteRenderer.color = _defaultColor;
        }
    }
}


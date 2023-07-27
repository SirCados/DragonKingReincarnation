using UnityEngine;

public class EnemyHurtbox : MonoBehaviour, IHurtbox
{
    bool _isOutOfHealth;
    
    bool _isRecoiling = false;
    bool _isArmorTooMuch = false;
    int _damageToTake;
    CharacterAttributes _attributes;
    
    [SerializeField] GameObject _corpseSprite;
    [SerializeField] GameObject _enemySprite;

    SpriteRenderer _spriteRenderer;
    Color _armorColor = new Color(20,20,20,255);
    Color _defaultColor = new Color(250, 250, 250, 255);


    private void Awake()
    {
        SetupEnemyHurtbox();
    }

    public void ToggleCorpse()
    {
        if (IsDead && _corpseSprite != null)
        {
            _enemySprite.SetActive(false);
            _corpseSprite.SetActive(true);
        }
    }

    void SetupEnemyHurtbox()
    {
        IsDead = false;
        IsArmorTooMuch = false;
        _enemySprite.SetActive(true);
        _spriteRenderer = _enemySprite.GetComponent<SpriteRenderer>();
        _defaultColor = _spriteRenderer.color;
        _corpseSprite.SetActive(false);
        _attributes = GetComponentInParent<CharacterAttributes>();
    }

    public void TakeHurt(int incomingDamage)
    {        
        _damageToTake = incomingDamage - _attributes.Armor;
        if(_damageToTake < 1)
        {
            IsArmorTooMuch = true;
            
            //ToggleArmorColorOn(true);
            _damageToTake = 0;
        }
        else
        {
            IsRecoiling = true;
            _attributes.CurrentHealth -= _damageToTake;

            if (_attributes.CurrentHealth <= 0)
            {
                _attributes.CurrentHealth = 0;
                IsDead = true;
            }
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

    public int DamageTaken
    {
        get => _damageToTake;
    }

    public void ToggleArmorColorOn(bool isArmorTooMuch)
    {
        if (isArmorTooMuch)
        {

            _spriteRenderer.color = Color.grey;
        }
        else
        {
            _spriteRenderer.color = _defaultColor;
        }
    }
}

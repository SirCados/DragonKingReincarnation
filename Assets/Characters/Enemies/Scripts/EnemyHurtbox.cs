using UnityEngine;

public class EnemyHurtbox : MonoBehaviour, IHurtbox
{
    public bool IsDead;
    public bool IsRecoiling = false;

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
        IsRecoiling = true;
        _character.CurrentHealth -= damageToTake;
        print("ow!");

        if (_character.CurrentHealth <= 0)
        {
            _character.CurrentHealth = 0;
            IsDead = true;
        }
    }
}

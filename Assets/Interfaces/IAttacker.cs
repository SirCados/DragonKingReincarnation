public interface IAttacker
{
    void Attack();
    void SpawnProjectile(UnityEngine.GameObject projectileToSpawn);
    int GetAttackDamage();//change to getter
    bool IsSpecial { get; set; }

    void RecievePower(int points);
}

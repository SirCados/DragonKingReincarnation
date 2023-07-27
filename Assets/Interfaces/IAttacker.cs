public interface IAttacker
{
    void Attack();
    void SpawnProjectile(UnityEngine.GameObject projectileToSpawn);
    int GetAttackDamage();
}

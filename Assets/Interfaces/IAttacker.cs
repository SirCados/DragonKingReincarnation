public interface IAttacker
{
    void BeginAttack();
    void SpawnProjectile(UnityEngine.GameObject projectileToSpawn);
    int GetAttackDamage();
}

public interface IAttacker
{
    void BeginAttack();
    void AttackRecovery(bool hasAttackHit);
    void SpawnProjectile(UnityEngine.GameObject projectileToSpawn);
    int GetAttackDamage();
}

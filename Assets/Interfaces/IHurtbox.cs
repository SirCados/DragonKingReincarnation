public interface IHurtbox
{
    bool IsRecoiling { get; set; }
    bool IsDead { get; set; }
    bool IsArmorTooMuch { get; set; }
    bool IsCorpse { get; }
    bool IsEaten { get; }
    int DamageTaken { get; }
    int GivePoints { get;}
    void TakeHurt(int damageToTake, bool isSpecial);
    void ToggleCorpse();
    void ToggleEaten();
    void ToggleHitColorOn(bool isInHitState);
}

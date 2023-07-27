public interface IHurtbox
{
    bool IsRecoiling { get; set; }
    bool IsDead { get; set; }
    bool IsArmorTooMuch { get; set; }
    int DamageTaken { get; }
    void TakeHurt(int damageToTake);
    void ToggleCorpse();
    void ToggleArmorColorOn(bool isArmorTooMuch);
}

namespace PromotionMod.Elements.PromotionAbilities.Gambler;

/// <summary>
/// </summary>
public class ActMeleeScatteringStrikes : ActMelee
{
    public override float BaseDmgMTP => 0.2f;
    public override bool UseWeaponDist => false;
    public override int PerformDistance => 99;
}
namespace PromotionMod.Elements.PromotionAbilities.Gambler;

public class ActMeleeRangeless : ActMelee
{
    public override bool UseWeaponDist => false;
    public override int PerformDistance => 99;
}
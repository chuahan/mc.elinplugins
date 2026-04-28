namespace PromotionMod.Elements.PromotionAbilities.Gambler;

public class ActMeleeVitalStrike : ActMelee
{
    public override bool UseWeaponDist => false;
    public override int PerformDistance => 99;
    public override bool AllowCounter => false;
    public override bool AllowParry => false;
    public override bool ShouldRollMax => true;
}
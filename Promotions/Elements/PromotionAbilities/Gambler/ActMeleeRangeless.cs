namespace PromotionMod.Elements;

public class ActMeleeRangeless : ActMelee
{
    public override bool UseWeaponDist => false;
    public override int PerformDistance => 99;
}
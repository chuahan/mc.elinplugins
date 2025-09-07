namespace PromotionMod.Elements.PromotionAbilities.Phantom;

public class ActWolkenkratzerMelee : ActMelee
{
    public override bool UseWeaponDist => false;
    public override float BaseDmgMTP => 0.7f;
    public override int PerformDistance => 99;
}
namespace PromotionMod.Elements.PromotionAbilities.Spellblade;

public class ActMeleeMyriadFleche : ActMelee
{
    public override bool AllowCounter => false;
    public override bool AllowParry => false;
    public override bool ShouldRollMax => true;
    public override bool UseWeaponDist => true;
}
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Phantom;

public class ActSchwarzeKatzeMelee : ActMelee
{
    public override bool UseWeaponDist => false;
    public override int PerformDistance => 99;
}
using PromotionMod.Common;
using PromotionMod.Stats.Hermit;
namespace PromotionMod.Elements.PromotionAbilities.Spellblade;

public class ActMeleeCrushingStrike : ActMelee
{
    public override bool AllowCounter => false;
    public override bool AllowParry => false;
    public override bool ShouldRollMax => true;
    public override bool UseWeaponDist => true;
    public override float BaseDmgMTP => 1.1F;
}
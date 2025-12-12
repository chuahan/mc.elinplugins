using PromotionMod.Common;
using PromotionMod.Stats.Hermit;
namespace PromotionMod.Elements.PromotionAbilities.Hermit;

public class ActMeleeVitalAssassination : ActMelee
{
    public override bool AllowCounter => false;
    public override bool AllowParry => false;
    public override bool ShouldRollMax => true;
    public override float BaseDmgMTP => 2F;
}
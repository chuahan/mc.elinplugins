using PromotionMod.Common;
using PromotionMod.Stats;

namespace PromotionMod.Elements;

public class ActCrushingStrike : PromotionCombatAbility
{
    public override int PromotionId => Constants.FeatSpellblade;
    public override string PromotionString => Constants.SpellbladeId;
    public override int AbilityId => Constants.ActCrushingStrikeId;

    public override PromotionAbilityCostType PromotionAbilityCost => PromotionAbilityCostType.PromotionAbilityCostMana;

    public override bool Perform()
    {
        CC.AddCondition<ConCrushingStrikeAttack>(GetPower(CC), true);
        return new ActMeleeCrushingStrike().Perform(CC, TC);
    }
}
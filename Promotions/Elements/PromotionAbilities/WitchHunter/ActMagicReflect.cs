using PromotionMod.Common;
using PromotionMod.Stats;

namespace PromotionMod.Elements;

public class ActMagicReflect : PromotionCombatAbility
{
    public override int PromotionId => Constants.FeatWitchHunter;
    public override string PromotionString => Constants.WitchHunterId;
    public override int AbilityId => Constants.ActMagicReflectId;

    public override PromotionAbilityCostType PromotionAbilityCost => PromotionAbilityCostType.PromotionAbilityCostMana;

    public override bool Perform()
    {
        CC.Chara.AddCondition<ConMagicReflect>(GetPower(CC));
        return true;
    }
}
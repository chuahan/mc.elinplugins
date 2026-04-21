using PromotionMod.Common;
using PromotionMod.Stats.WarCleric;
namespace PromotionMod.Elements.PromotionAbilities.WarCleric;

public class ActBlessedArmaments : PromotionSpellAbility
{
    public override int PromotionId => Constants.FeatWarCleric;
    public override string PromotionString => Constants.WarClericId;
    public override int AbilityId => Constants.ActBlessedArmamentsId;

    public override bool Perform()
    {
        CC.AddCondition<ConShiningBlade>(GetPower(CC));
        return true;
    }
}
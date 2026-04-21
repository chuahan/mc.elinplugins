using PromotionMod.Common;
using PromotionMod.Stats.Sentinel;
namespace PromotionMod.Elements.PromotionAbilities.Sentinel;

public class StRestraint : PromotionAbility
{
    public override int PromotionId => Constants.FeatSentinel;
    public override string PromotionString => Constants.SentinelId;
    public override int AbilityId => Constants.StRestraintId;

    public override bool Perform()
    {
        CC.RemoveCondition<StanceRage>();
        CC.AddCondition<StanceRestraint>();
        return true;
    }
}
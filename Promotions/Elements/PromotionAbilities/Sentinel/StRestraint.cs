using PromotionMod.Common;
using PromotionMod.Stats;

namespace PromotionMod.Elements;

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
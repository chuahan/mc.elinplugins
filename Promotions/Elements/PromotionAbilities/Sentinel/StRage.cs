using PromotionMod.Common;
using PromotionMod.Stats.Sentinel;
namespace PromotionMod.Elements.PromotionAbilities.Sentinel;

public class StRage : PromotionAbility
{
    public override int PromotionId => Constants.FeatSentinel;
    public override string PromotionString => Constants.SentinelId;
    public override int AbilityId => Constants.StRageId;

    public override bool Perform()
    {
        // Snapshots PV, including StanceRestraint
        int snapshotPV = CC.PV;
        CC.RemoveCondition<StanceRestraint>();
        CC.AddCondition<StanceRage>(snapshotPV);
        return true;
    }
}
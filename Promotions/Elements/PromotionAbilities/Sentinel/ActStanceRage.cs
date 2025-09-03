using PromotionMod.Common;
using PromotionMod.Stats.Sentinel;
namespace PromotionMod.Elements.PromotionAbilities.Sentinel;

public class ActStanceRage : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatSentinel) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.SentinelId.lang()));
            return false;
        }
        return base.CanPerform();
    }
    
    public override bool Perform()
    {
        // Snapshots PV, including StanceRestraint
        int snapshotPV = CC.PV;
        CC.RemoveCondition<StanceRestraint>();
        CC.AddCondition<StanceRage>(snapshotPV);
        return true;
    }
}
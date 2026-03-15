using PromotionMod.Common;
using PromotionMod.Stats.Sentinel;
namespace PromotionMod.Elements.PromotionAbilities.Sentinel;

public class StRestraint : Ability
{
    public override bool CanPerform()
    {
        if (!CC.MatchesPromotion(Constants.FeatSentinel))
        {
            Msg.Say("classlocked_ability".lang(Constants.SentinelId.lang()));
            return false;
        }
        return base.CanPerform();
    }

    public override bool Perform()
    {
        CC.RemoveCondition<StanceRage>();
        CC.AddCondition<StanceRestraint>();
        return true;
    }
}
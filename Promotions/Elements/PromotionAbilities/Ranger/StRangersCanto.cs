using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Ranger;

public class StRangersCanto : Ability
{
    public override bool CanPerform()
    {
        if (!CC.MatchesPromotion(Constants.FeatRanger))
        {
            Msg.Say("classlocked_ability".lang(Constants.RangerId.lang()));
            return false;
        }
        // CC must be riding or be a parasite.
        if (CC.ride == null) return false;
        return base.CanPerform();
    }
}
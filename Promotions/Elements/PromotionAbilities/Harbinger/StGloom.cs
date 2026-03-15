using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Harbinger;

public class StGloom : Ability
{
    public override bool CanPerform()
    {
        if (!CC.MatchesPromotion(Constants.FeatHarbinger))
        {
            Msg.Say("classlocked_ability".lang(Constants.HarbingerId.lang()));
            return false;
        }

        return base.CanPerform();
    }
}
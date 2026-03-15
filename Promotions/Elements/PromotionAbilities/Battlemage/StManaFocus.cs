using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Battlemage;

public class StManaFocus : Ability
{
    public override bool CanPerform()
    {
        if (!CC.MatchesPromotion(Constants.FeatBattlemage))
        {
            Msg.Say("classlocked_ability".lang(Constants.BattlemageId.lang()));
            return false;
        }

        return base.CanPerform();
    }
}
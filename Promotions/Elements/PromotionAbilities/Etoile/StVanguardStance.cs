using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Luminary;

public class StVanguardStanceOld : Ability
{
    public override bool CanPerform()
    {
        if (!CC.MatchesPromotion(Constants.FeatEtoile))
        {
            Msg.Say("classlocked_ability".lang(Constants.EtoileId.lang()));
            return false;
        }
        return base.CanPerform();
    }
}
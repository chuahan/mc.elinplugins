using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Sharpshooter;

public class StOverwatch : Ability
{
    public override bool CanPerform()
    {
        if (!CC.MatchesPromotion(Constants.FeatSharpshooter))
        {
            Msg.Say("classlocked_ability".lang(Constants.SharpshooterId.lang()));
            return false;
        }
        return base.CanPerform();
    }
}
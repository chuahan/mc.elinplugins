using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Sharpshooter;

public class StOverwatch : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatSharpshooter) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.SharpshooterId.lang()));
            return false;
        }
        return base.CanPerform();
    }
}
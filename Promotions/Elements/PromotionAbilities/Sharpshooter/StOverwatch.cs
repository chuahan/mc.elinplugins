using PromotionMod.Common;
using PromotionMod.Stats.Sharpshooter;
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

    public override bool Perform()
    {
        CC.AddCondition<StanceOverwatch>();
        return true;
    }
}
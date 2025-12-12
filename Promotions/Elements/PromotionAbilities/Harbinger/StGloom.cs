using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Harbinger;

public class StGloom : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatHarbinger) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.HarbingerId.lang()));
            return false;
        }

        return base.CanPerform();
    }
}
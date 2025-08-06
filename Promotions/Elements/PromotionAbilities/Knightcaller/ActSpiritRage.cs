using PromotionMod.Common;
using PromotionMod.Stats.Knightcaller;
namespace PromotionMod.Elements.PromotionAbilities.Knightcaller;

public class ActSpiritRage : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatKnightcaller) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.KnightcallerId.lang()));
            return false;
        }
        if (CC.HasCooldown(Constants.ActSpiritRageId)) return false;
        return base.CanPerform();
    }

    public override bool Perform()
    {
        foreach (Chara target in HelperFunctions.GetCharasWithinRadius(CC.pos, 5F, CC, true, true))
        {
            if (target.IsPCPartyMinion)
            {
                target.AddCondition<ConSpiritRage>();
            }
        }

        CC.AddCooldown(Constants.ActSpiritRageId, 10);
        return true;
    }
}
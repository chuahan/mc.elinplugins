using PromotionMod.Common;
using PromotionMod.Stats.Knightcaller;
namespace PromotionMod.Elements.PromotionAbilities.Knightcaller;

public class ActSpiritRage : PromotionSpellAbility
{
    public override int PromotionId => Constants.FeatKnightcaller;
    public override string PromotionString => Constants.KnightcallerId;
    public override int AbilityId => Constants.ActSpiritRageId;

    public override bool Perform()
    {
        foreach (Chara target in HelperFunctions.GetCharasWithinRadius(CC.pos, 5F, CC, true, true))
        {
            if (target.IsPCPartyMinion || target == CC)
            {
                target.AddCondition<ConSpiritRage>(GetPower(CC));
            }
        }
        return true;
    }
}
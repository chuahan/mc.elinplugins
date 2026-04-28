using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Knightcaller;

public class ActSpiritMobilize : PromotionSpellAbility
{
    public override int PromotionId => Constants.FeatKnightcaller;
    public override string PromotionString => Constants.KnightcallerId;
    public override int AbilityId => Constants.ActSpiritMobilizeId;

    public override bool CanPerformExtra(bool verbose)
    {
        return TP.IsValid && Los.IsVisible(CC.pos, TP);
    }

    public override bool Perform()
    {
        int power = GetPower(CC);
        foreach (Chara target in HelperFunctions.GetCharasWithinRadius(CC.pos, 10F, CC, true, true))
        {
            if (target.IsPCPartyMinion)
            {
                target.Teleport(TP.GetNearestPoint(false, false) ?? TP);
                target.AddCondition<ConHero>(power);
            }
        }
        return true;
    }
}
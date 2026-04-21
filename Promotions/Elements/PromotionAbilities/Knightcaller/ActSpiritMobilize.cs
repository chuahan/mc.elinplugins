using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Knightcaller;

public class ActSpiritMobilize : PromotionSpellAbility
{
    public override int PromotionId => Constants.FeatKnightcaller;
    public override string PromotionString => Constants.KnightcallerId;
    public override int Cooldown => 5;
    public override int AbilityId => Constants.ActSpiritMobilizeId;

    public override bool CanPerformExtra()
    {
        if (!TP.IsValid || !Los.IsVisible(CC.pos, TP)) return false;
        return base.CanPerform();
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

        CC.AddCooldown(AbilityId, Cooldown);
        return true;
    }
}
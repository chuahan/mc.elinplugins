using PromotionMod.Common;
using PromotionMod.Stats.Knightcaller;
namespace PromotionMod.Elements.PromotionAbilities.Knightcaller;

public class ActSpiritMobilize : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatKnightcaller) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.KnightcallerId.lang()));
            return false;
        }
        if (CC.HasCooldown(Constants.ActSpiritMobilizeId)) return false;
        if (!TP.IsValid || !Los.IsVisible(CC.pos, TP)) return false;
        return base.CanPerform();
    }

    public override Cost GetCost(Chara c)
    {
        Cost convertToMp = base.GetCost(c);
        convertToMp.type = CostType.MP;
        return convertToMp;
    }

    public override bool Perform()
    {
        int power = this.GetPower(CC);
        foreach (Chara target in HelperFunctions.GetCharasWithinRadius(CC.pos, 10F, CC, true, true))
        {
            if (target.IsPCPartyMinion)
            {
                target.Teleport(TP.GetNearestPoint(false, false) ?? TP);
                target.AddCondition<ConHero>(power);
            }
        }

        CC.AddCooldown(Constants.ActSpiritMobilizeId, 5);
        return true;
    }
}
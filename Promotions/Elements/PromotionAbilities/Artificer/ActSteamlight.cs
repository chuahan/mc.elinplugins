using PromotionMod.Common;
using PromotionMod.Stats.Artificer;
namespace PromotionMod.Elements.PromotionAbilities.Artificer;

public class ActSteamlight : Ability
{
    public override Cost GetCost(Chara c)
    {
        return new Cost
        {
            cost = 1,
            type = CostType.SP
        };
    }

    public override bool CanPerform()
    {
        if (!CC.HasTag(CTAG.machine)) return false;
        if (CC.HasCondition<ConBurnout>()) return false;
        if (CC.things.Find(Constants.ArtificerSteamlightItem) == null) return false;

        return base.CanPerform();
    }

    public override bool Perform()
    {
        CC.AddCondition<ConSteamlight>();
        CC.things.Find(Constants.ArtificerSteamlightItem).Split(1).Destroy();
        return true;
    }
}
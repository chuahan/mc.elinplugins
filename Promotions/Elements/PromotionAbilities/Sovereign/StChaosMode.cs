using PromotionMod.Common;
using PromotionMod.Stats.Sovereign;
namespace PromotionMod.Elements.PromotionAbilities.Sovereign;

public class StChaosMode : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatSovereign) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.SovereignId.lang()));
            return false;
        }
        // Both Sovereign Modes share a cooldown.
        if (CC.HasCooldown(Constants.StLawModeId)) return false;
        return base.CanPerform();
    }

    public override Cost GetCost(Chara c)
    {
        return new Cost
        {
            type = CostType.None,
            cost = 0
        };
    }

    public override bool Perform()
    {
        CC.Talk("sovereign_chaos".langList().RandomItem());
        CC.RemoveCondition<StanceLawSovereign>();
        CC.AddCondition<StanceChaosSovereign>();
        CC.AddCooldown(Constants.StLawModeId, 5);

        return true;
    }
}
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
        if (CC.HasCooldown(Constants.StChaosModeId)) return false;
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
        CC.SayRaw($"sovereign_chaos_{EClass.rnd(5)}".langGame());
        CC.RemoveCondition<StanceLawSovereign>();
        CC.AddCondition<StanceChaosSovereign>();
        CC.AddCooldown(Constants.StLawModeId, 5);
        CC.AddCooldown(Constants.StChaosModeId, 5);
        return true;
    }
}
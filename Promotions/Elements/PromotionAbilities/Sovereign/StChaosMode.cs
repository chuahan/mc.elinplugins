using PromotionMod.Common;
using PromotionMod.Stats.Sovereign;
namespace PromotionMod.Elements.PromotionAbilities.Sovereign;

public class StChaosMode : PromotionAbility
{
    public override int PromotionId => Constants.FeatSovereign;
    public override string PromotionString => Constants.SovereignId;
    public override int Cooldown => 5;
    public override int AbilityId => Constants.StChaosModeId;

    public override PromotionAbilityCostType PromotionAbilityCost => PromotionAbilityCostType.PromotionAbilityCostNone;

    public override bool Perform()
    {
        CC.SayRaw($"sovereign_chaos_{EClass.rnd(5)}".langGame());
        CC.RemoveCondition<StanceLawSovereign>();
        CC.AddCondition<StanceChaosSovereign>();
        CC.AddCooldown(Constants.StLawModeId, Cooldown);
        CC.AddCooldown(Constants.StChaosModeId, Cooldown);
        return true;
    }
}
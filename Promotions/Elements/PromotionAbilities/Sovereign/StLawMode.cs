using PromotionMod.Common;
using PromotionMod.Stats.Sovereign;
namespace PromotionMod.Elements.PromotionAbilities.Sovereign;

public class StLawMode : PromotionAbility
{
    public override int PromotionId => Constants.FeatSovereign;
    public override string PromotionString => Constants.SovereignId;
    public int Cooldown => 0;
    public override int AbilityId => Constants.StLawModeId;

    public override PromotionAbilityCostType PromotionAbilityCost => PromotionAbilityCostType.PromotionAbilityCostNone;

    public override bool Perform()
    {
        CC.SayRaw($"sovereign_law_{EClass.rnd(5)}".langGame());
        CC.RemoveCondition<StanceChaosSovereign>();
        CC.AddCondition<StanceLawSovereign>();
        CC.AddCooldown(Constants.StLawModeId, Cooldown);
        CC.AddCooldown(Constants.StChaosModeId, Cooldown);
        return true;
    }
}
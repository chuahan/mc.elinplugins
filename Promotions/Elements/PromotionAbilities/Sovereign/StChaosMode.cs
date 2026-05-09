using PromotionMod.Common;
using PromotionMod.Stats.Sovereign;
namespace PromotionMod.Elements.PromotionAbilities.Sovereign;

public class StChaosMode : PromotionAbility
{
    public override int PromotionId => Constants.FeatSovereign;
    public override string PromotionString => Constants.SovereignId;
    public int Cooldown => 5;
    public override int AbilityId => Constants.StChaosModeId;

    public override PromotionAbilityCostType PromotionAbilityCost => PromotionAbilityCostType.PromotionAbilityCostNone;

    public override bool Perform()
    {
        if (EClass.rnd(5) == 0) CC.SayRaw($"sovereign_chaos_{EClass.rnd(5)}".langGame());
        CC.AddCondition<StanceChaosSovereign>();
        if (!CC.IsPC) // Force NPCs to have 4x the cooldown so they remain in that stance for a bit longer.
        {
            CC.AddCooldown(Constants.StLawModeId, Cooldown * 4);
            CC.AddCooldown(Constants.StChaosModeId, Cooldown * 4);
        }
        else
        {
            CC.AddCooldown(Constants.StLawModeId, Cooldown);
            CC.AddCooldown(Constants.StChaosModeId, Cooldown);
        }
        
        return true;
    }
}
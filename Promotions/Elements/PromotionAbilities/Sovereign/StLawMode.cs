using PromotionMod.Common;
using PromotionMod.Stats.Sovereign;

namespace PromotionMod.Elements.PromotionAbilities.Sovereign;

public class StLawMode : Ability
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
        return base.CanPerform();
    }
    
    public override Cost GetCost(Chara c)
    {
        return new Cost()
        {
            type = CostType.None,
            cost = 0,
        };
    }
    
    public override bool Perform()
    {
        CC.SayRaw("sovereign_law".langList().RandomItem());
        CC.RemoveCondition<StanceChaosSovereign>();
        CC.AddCondition<StanceLawSovereign>();
        CC.AddCooldown(Constants.StLawModeId, 5);
        
        return true;
    }
}
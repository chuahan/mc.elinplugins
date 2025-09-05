using System;
using PromotionMod.Common;
using PromotionMod.Stats.Sovereign;

namespace PromotionMod.Elements.PromotionAbilities.Sovereign;

public abstract class ActSovereignOrder : Ability
{
    protected abstract string OrderType { get; }
    protected abstract int CooldownId { get; }
    public abstract void AddLawCondition(Chara chara, int stacks);
    public abstract void AddChaosCondition(Chara chara, int stacks);
    
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatSovereign) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.SovereignId.lang()));
            return false;
        }
        if (CC.HasCooldown(CooldownId)) return false;
        if (!CC.HasCondition<StanceSovereign>()) return false;
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
        // We assume one of the two stances is present and mutually exclusive.
        int stacks = 0;
        bool isLaw = false;

        foreach (Condition condition in CC.conditions)
        {
            if (condition is StanceLawSovereign law)
            {
                stacks = law.Stacks;
                isLaw = true;
                break;
            }
            else if (condition is StanceChaosSovereign chaos)
            {
                stacks = chaos.Stacks;
                break;
            }
        }

        string actionString = "sovereign_" + OrderType + (isLaw ? "_law" : "_chaos");
        CC.SayRaw("actionString".langList().RandomItem());
        
        // Apply Order effect to nearby allies
        foreach (Chara ally in HelperFunctions.GetCharasWithinRadius(CC.pos, 5F, CC, true, false))
        {
            if (isLaw)
                AddLawCondition(ally, stacks);
            else
                AddChaosCondition(ally, stacks);
        }

        CC.AddCooldown(CooldownId, 10);
        return true;
    }
}
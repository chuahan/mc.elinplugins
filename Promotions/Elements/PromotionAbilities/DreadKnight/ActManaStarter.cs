using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Stats;
using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.DreadKnight;

public class ActManaStarter : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatDreadKnight) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.DreadKnightId.lang()));
            return false;
        }

        if (GetHPCost(CC) > CC.hp)
        {
            if (CC.IsPC) Msg.Say("dreadknight_notenoughhp".lang());
            return false;
        }

        return base.CanPerform();
    }

    public override Cost GetCost(Chara c)
    {
        return new Cost
        {
            cost = 0,
            type = CostType.None
        };
    }

    public int GetHPCost(Chara c)
    {
        ConDarkTraces darkTrace = c.GetCondition<ConDarkTraces>();
        float hpCost = 0.1F;
        if (darkTrace != null)
        {
            // Dark Traces increases the HP Cost by 10% each stack
            hpCost += darkTrace.GetStacks() * .1F;
        }

        return (int)(c.MaxHP * hpCost);
    }
    
    public override bool Perform()
    {
        ConDarkTraces darkTrace = c.GetCondition<ConDarkTraces>();
        float hpCost = 0.1F;
        if (darkTrace != null)
        {
            // Dark Traces increases the HP Cost by 10% each stack
            hpCost += darkTrace.GetStacks() * .1F;
        }
        else
        {
            darkTrace = CC.AddCondition<ConDarkTraces>();
        }

        // User will restore 2x the cost in Mana.
        int cost = (int)(CC.MaxHP * hpCost);
        CC.DamageHP(cost, AttackSource.Condition);
        CC.mana.Mod(cost * 2);
        
        darkTrace.AddStacks(1);
        return true;
    }
}
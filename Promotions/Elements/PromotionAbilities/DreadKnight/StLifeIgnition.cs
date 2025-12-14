using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Stats;
using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.DreadKnight;

public class StLifeIgnition : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatDreadKnight) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.DreadKnightId.lang()));
            return false;
        }
        
        if (CC != null)
        {
            int hpCost = (int)(CC.MaxHP * 0.1F);
            if (CC.hp <= hpCost)
            {
                // You would die if you use this now.
                if (CC.IsPC) Msg.Say("dreadknight_notenoughhp".lang());
                return false;
            }
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
    
    public override bool Perform()
    {
        CC.AddCondition<StanceLifeIgnition>();
        return true;
    }
}
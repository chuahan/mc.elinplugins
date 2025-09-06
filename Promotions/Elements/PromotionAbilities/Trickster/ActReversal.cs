using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Stats;

namespace PromotionMod.Elements.PromotionAbilities.Trickster;

/// <summary>
/// Consumes debuffs, provides self and allies Protection scaled to the amount of debuffs on the target.
/// </summary>
public class ActReversal : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatTrickster) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.TricksterId.lang()));
            return false;
        }

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
        List<Condition> negativeConditions = TC.Chara.conditions.Where(con => con.Type is ConditionType.Bad or ConditionType.Debuff).ToList();
        if (negativeConditions.Count == 0)
        {
            CC.SayNothingHappans();
        }
        else
        {
            int protection = HelperFunctions.SafeDice(Constants.TricksterReversalAlias, this.GetPower(CC));
            protection = ConProtection.CalcProtectionAmount(HelperFunctions.SafeMultiplier(protection, negativeConditions.Count));
            foreach (Condition con in negativeConditions) con.Kill();
            foreach (Chara ally in HelperFunctions.GetCharasWithinRadius(CC.pos, 5f, CC, true, false))
            {
                if (ally.HasCondition<ConProtection>())
                {
                    ally.GetCondition<ConProtection>().AddProtection(protection);
                }
                else
                {
                    ally.AddCondition<ConProtection>(protection);
                }
            }
        }

        return true;
    }
}
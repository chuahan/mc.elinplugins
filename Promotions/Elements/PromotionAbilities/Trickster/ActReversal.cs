using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Stats;
namespace PromotionMod.Elements.PromotionAbilities.Trickster;

/// <summary>
///     Consumes debuffs, provides self and allies Protection scaled to the amount of debuffs on the target.
/// </summary>
public class ActReversal : PromotionSpellAbility
{
    public override int PromotionId => Constants.FeatTrickster;
    public override string PromotionString => Constants.TricksterId;
    public override int AbilityId => Constants.ActReversalId;

    public override bool Perform()
    {
        List<Condition> negativeConditions = TC.Chara.conditions.Where(con => con.Type is ConditionType.Debuff).ToList();
        if (negativeConditions.Count == 0)
        {
            CC.SayNothingHappans();
        }
        else
        {
            int protection = HelperFunctions.SafeDice(Constants.TricksterReversalAlias, GetPower(CC));
            protection = HelperFunctions.SafeMultiplier(protection, negativeConditions.Count);
            foreach (Condition con in negativeConditions)
            {
                con.Kill();
            }
            foreach (Chara ally in HelperFunctions.GetCharasWithinRadius(CC.pos, 5f, CC, true, false))
            {
                ally.AddCondition<ConProtection>(protection);
            }
        }

        return true;
    }
}
using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Stats;
using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.Elementalist;

/// <summary>
///     Elementalist Ability
///     Consumes all orbs and apply Elemental Storm to yourself based on how many elements you had.
/// </summary>
public class ActElementalFury : Ability
{
    public int ElementalFuryRequirement = 4;

    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatElementalist) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.ElementalistId.lang()));
            return false;
        }
        if (CC.HasCondition<ConElementalist>())
        {
            ConElementalist elementalist = CC.GetCondition<ConElementalist>();
            if (elementalist.GetElementalCombination() < ElementalFuryRequirement) return false;
        }
        else
        {
            return false;
        }

        return base.CanPerform();
    }

    public override Cost GetCost(Chara c)
    {
        Cost cost = base.GetCost(c);
        cost.type = CostType.MP;
        return cost;
    }

    // Apply Spell Enhance to this ability.
    public override int GetPower(Card c)
    {
        int power = base.GetPower(c);
        return power * Mathf.Max(100 + c.Evalue(411) - c.Evalue(93), 1) / 100;
    }

    public override bool Perform()
    {
        ConElementalist elementalist = CC.GetCondition<ConElementalist>();
        // Clone the Elemental Stockpile to deplete.
        ConElementalFury fury = CC.AddCondition<ConElementalFury>(GetPower(CC)) as ConElementalFury;
        fury.ElementalStockpile = new Dictionary<int, int>(elementalist.ElementalStockpile);
        elementalist.ConsumeElementalOrbs();
        return true;
    }
}
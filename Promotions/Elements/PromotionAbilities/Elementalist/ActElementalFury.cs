using System;
using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Stats;
namespace PromotionMod.Elements.PromotionAbilities.Elementalist;

/// <summary>
/// Elementalist Ability
/// Consumes all orbs and apply Elemental Storm to yourself based on how many elements you had.
/// </summary>
public class ActElementalFury : Ability
{
    public int ElementalFuryRequirement = 4;
    public override Cost GetCost(Chara c)
    {
        Cost cost = base.GetCost(c);
        cost.type = CostType.MP;
        return cost;
    }

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

    public override bool Perform()
    {
        ConElementalist elementalist = CC.GetCondition<ConElementalist>();
        // Increases duration with your variety count. 
        int elementalCombo = 0;
        List<int> activeElements = new List<int>();
        foreach (KeyValuePair<int, int> elementOrb in elementalist.ElementalStockpile)
        {
            if (elementalist.ElementalStockpile[elementOrb.Key] > 0)
            {
                elementalCombo++;
                activeElements.Add(elementOrb.Key);
            }
        }
        ConElementalFury fury = CC.AddCondition<ConElementalFury>(GetPower(CC)) as ConElementalFury;
        if (fury != null)
        {
            fury.Mod(elementalCombo / 2);
            fury.Stacks = Math.Min(10, elementalist.GetElementalStrength());
            // At minimum, if you have 4 elements you will Fury for the base 3 ticks.
            // Additional 1/2/4/6/8 turns.
            fury.ElementsToUse.AddRange(activeElements);
        }

        return true;
    }
}
using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Stats;
using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.Elementalist;

/// <summary>
///     Elementalist Ability
///     Consumes all orbs and drops elemental meteors on the enemy's location.
/// </summary>
public class ActElementalExtinction : Ability
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
        int power = GetPower(CC);
        ConElementalist elementalist = CC.GetCondition<ConElementalist>();
        // Take all active elements.
        Dictionary<int, int> activeStockpile = elementalist.ElementalStockpile.Where(pair => pair.Value > 0).ToDictionary(pair => pair.Key, pair => pair.Value);

        while (activeStockpile.Count > 0)
        {
            int element = activeStockpile.Keys.RandomItem();
            if (!TC.IsAliveInCurrentZone)
            {
                // Try to reacquire a target.
                List<Chara> nearbyEnemies = HelperFunctions.GetCharasWithinRadius(TP, 3F, CC, false, false);
                if (nearbyEnemies.Count == 0)
                {
                    elementalist.ConsumeElementalOrbs();
                    return true;
                }
                TC = nearbyEnemies.RandomItem();
            }
            ActRef actRef = default(ActRef);
            actRef.origin = CC;
            actRef.aliasEle = Constants.ElementAliasLookup[element];
            ActEffect.ProcAt(EffectId.Meteor, power, BlessedState.Normal, CC, TC, TC.pos, true, actRef);

            // Consume an orb.
            activeStockpile[element]--;
            if (activeStockpile[element] <= 0) activeStockpile.Remove(element);
        }

        elementalist.ConsumeElementalOrbs();
        return true;
    }
}
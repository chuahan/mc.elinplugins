using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Stats;
namespace PromotionMod.Elements.PromotionAbilities.Elementalist;

/// <summary>
///     Elementalist Ability
///     Consumes all orbs and drops elemental meteors on the enemy's location.
/// </summary>
public class ActElementalExtinction : PromotionSpellAbility
{

    public int ElementalFuryRequirement = 4;
    public override int PromotionId => Constants.FeatElementalist;
    public override string PromotionString => Constants.ElementalistId;
    public override int AbilityId => Constants.ActElementalExtinctionId;

    public override bool CanPerformExtra(bool verbose)
    {
        if (CC.HasCondition<ConElementalist>())
        {
            ConElementalist elementalist = CC.GetCondition<ConElementalist>();
            if (elementalist.GetElementalCombination() < ElementalFuryRequirement)
            {
                if (CC.IsPC && verbose) Msg.Say("elementalist_notenoughorbs".langGame(ElementalFuryRequirement.ToString()));
                return false;
            }
        }
        else
        {
            return false;
        }

        return true;
    }

    public override bool Perform()
    {
        int power = GetPower(CC);
        ConElementalist elementalist = CC.GetCondition<ConElementalist>();
        // Take all active elements.
        Dictionary<int, int> activeStockpile = elementalist.ElementalStockpile.Where(pair => pair.Value > 0).ToDictionary(pair => pair.Key, pair => pair.Value);

        int orbsConsumed = 0;
        while (activeStockpile.Count > 0)
        {
            int element = activeStockpile.Keys.RandomItem();
            if (!TC.IsAliveInCurrentZone)
            {
                // Try to reacquire a target.
                List<Chara> nearbyEnemies = HelperFunctions.GetCharasWithinRadius(TP, 3F, CC, false, false);
                if (nearbyEnemies.Count == 0)
                {
                    // No targets left, finish consuming.
                    elementalist.ConsumeElementalOrbs();
                    return true;
                }
                TC = nearbyEnemies.RandomItem();
            }
            ActRef actRef = default(ActRef);
            actRef.act = this;
            actRef.origin = CC;
            actRef.aliasEle = Constants.ElementAliasLookup[element];

            Element eleObj = Element.Create(Constants.ElementAliasLookup[element], power / 10);
            // Go straight to Damage ele to focus fire meteors on that location.
            // Need to well... draw em though.
            EffectMeteor.Create(TC.pos, 1, 1, delegate
            {
            });
            CC.PlaySound("spell_ball");
            if (CC.IsInMutterDistance())
            {
                Shaker.ShakeCam("ball");
            }
            EClass.Wait(1f, CC);
            ActEffect.DamageEle(CC, EffectId.Meteor, power, eleObj, new List<Point>
            {
                TC.pos
            }, actRef, nameof(ActElementalExtinction));

            // Consume an orb.
            activeStockpile[element]--;
            if (activeStockpile[element] <= 0) activeStockpile.Remove(element);
            orbsConsumed++;
        }

        int spellExp = CC.elements.GetSpellExp(CC, act) * orbsConsumed;
        CC.ModExp(AbilityId, spellExp);

        elementalist.ConsumeElementalOrbs();
        return true;
    }
}
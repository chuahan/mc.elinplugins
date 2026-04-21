using System;
using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Stats;
using PromotionMod.Stats.Headhunter;
namespace PromotionMod.Elements.PromotionAbilities.Headhunter;

/// <summary>
///     Headhunter Ability
///     AOE Melee attack that strikes all nearby enemies.
///     Increases damage if enemy is full HP.
///     Increases damage if enemy is afflicted with Bad Conditions.
///     Inflicts PV reduction.
/// </summary>
public class ActReap : PromotionCombatAbility
{
    public override int PromotionId => Constants.FeatHeadhunter;
    public override string PromotionString => Constants.HeadhunterId;
    public override int Cooldown => 10;
    public override int AbilityId => Constants.ActReapId;

    public override int PerformDistance => 2;

    public override bool CanPerform()
    {
        if (CC == TC || TC is not { isChara: true } || CC.Dist(TC) > PerformDistance)
        {
            return false;
        }
        return true;
    }

    // Cost is reduced by 10% per Headhunter stack are active.
    public override Cost GetCost(Chara c)
    {
        Cost cost = base.GetCost(c);
        if (CC != null && CC.HasCondition<ConHeadhunter>())
        {
            int reduction = (int)(cost.cost * 0.1);
            cost.cost -= CC.GetCondition<ConHeadhunter>().value * reduction;
            cost.cost = Math.Min(1, cost.cost);
        }
        return cost;
    }

    public override bool Perform()
    {
        float num = 0f;
        HashSet<int> hashSet = new HashSet<int>();
        foreach (Chara target in HelperFunctions.GetCharasWithinRadius(CC.pos, PerformDistance, CC, false, true))
        {
            Point pos = target.pos;
            if (!hashSet.Contains(pos.index))
            {
                hashSet.Add(pos.index);
                TweenUtil.Delay(num, delegate
                {
                    pos.PlayEffect("ab_swarm");
                    pos.PlaySound("ab_swarm");
                });
                if (num < 1f)
                {
                    num += 0.07f;
                }
            }
            new ActMeleeReap().Perform(CC, target);
            int breakAmount = (int)HelperFunctions.SigmoidScaling(GetPower(CC), 10, 25);
            target.AddCondition(SubPoweredCondition.Create(nameof(ConArmorBreak), GetPower(CC), breakAmount));
        }
        CC.AddCooldown(AbilityId, Cooldown);
        return true;
    }
}
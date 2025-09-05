using System;
using System.Collections.Generic;
using System.Runtime;
using HarmonyLib;
using PromotionMod.Common;
using PromotionMod.Elements.PromotionFeats;
using PromotionMod.Stats;
using PromotionMod.Stats.Sentinel;
using PromotionMod.Stats.Sovereign;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(ActMelee))]
public class ActMeleePatches
{
    [HarmonyPatch(nameof(ActMelee.Attack), typeof(Card), typeof(Point), typeof(float), typeof(bool))]
    [HarmonyPrefix]
    public static bool AttackPrefixPatch(ActMelee __instance, Card _tc, Point _tp, ref float mtp, bool subAttack)
    {
        // Sentinel - If the target is a Sentinel with a Shield, they have a chance to block the blow, reducing the damage.
        if (_tc.Chara.Evalue(Constants.FeatSentinel) > 0 && _tc.Chara.body.GetAttackStyle() == AttackStyle.Shield && !Act.TC.IsDisabled && !Act.TC.IsRestrainedResident)
        {
            // Shield Skill * 2 + 50
            int blockChance = _tc.Evalue(123) * 2 + 50;
            if (EClass.rnd(blockChance) > 100)
            {
                // Maximum damage reduction of 85%. Applied as a damage multiplier.
                float blockPercent = (100 - Math.Min(FeatSentinel.GetShieldPower(_tc.Chara), 85)) / 100F;
                mtp = blockPercent;
            }
        }
        
        // Sentinel - If the attacker is a Sentinel in Rage Stance, add a multiplier of damage based off of their PV difference.
        if (Act.CC.Evalue(Constants.FeatSentinel) > 0 && Act.CC.HasCondition<StanceRage>())
        {
            // Every 5 PV adds 1% more damage.
            StanceRage stance = Act.CC.GetCondition<StanceRage>();
            mtp += stance.power / 5F;
        }

        return true;
    }

    [HarmonyPatch(nameof(ActMelee.CanPerform))]
    [HarmonyPrefix]
    public static bool CanPerformPatch(ref bool __result)
    {
        if (Act.CC.HasCondition<ConDisable>())
        {
            __result = false;
            return false;
        }
        return true;
    }
}
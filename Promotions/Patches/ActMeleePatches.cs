using System;
using HarmonyLib;
using PromotionMod.Common;
using PromotionMod.Elements.PromotionFeats;
using PromotionMod.Stats;
using PromotionMod.Stats.Sentinel;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(ActMelee))]
public class ActMeleePatches
{
    [HarmonyPatch(nameof(ActMelee.Attack), typeof(float))]
    [HarmonyPrefix]
    public static bool AttackPrefixPatch(ActMelee __instance, ref float dmgMulti)
    {
        // Sentinel - If the target is a Sentinel with a Shield, they have a chance to block the blow, reducing the damage.
        if (Act.TC.Chara.Evalue(Constants.FeatSentinel) > 0 && Act.TC.Chara.body.GetAttackStyle() == AttackStyle.Shield && !Act.TC.IsDisabled && !Act.TC.IsRestrainedResident)
        {
            // Shield Skill * 2 + 50
            int blockChance = Act.TC.Evalue(123) * 2 + 50;
            if (EClass.rnd(blockChance) > 100)
            {
                // Maximum damage reduction of 85%. Applied as a damage multiplier.
                float blockPercent = (100 - Math.Min(FeatSentinel.GetShieldPower(Act.TC.Chara), 85)) / 100F;
                dmgMulti = blockPercent;
            }
        }

        // Sentinel - If the attacker is a Sentinel in Rage Stance, add a multiplier of damage based off of their PV difference.
        if (Act.CC.Evalue(Constants.FeatSentinel) > 0 && Act.CC.HasCondition<StanceRage>())
        {
            // Every 5 PV adds 1% more damage.
            StanceRage stance = Act.CC.GetCondition<StanceRage>();
            dmgMulti += stance.power / 5F;
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
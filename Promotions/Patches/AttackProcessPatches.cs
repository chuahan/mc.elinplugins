using System.Linq;
using HarmonyLib;
using PromotionMod.Common;
using PromotionMod.Stats.Berserker;
using PromotionMod.Stats.Hermit;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(AttackProcess))]
public class AttackProcessPatches
{
    [HarmonyPatch(nameof(AttackProcess.CalcHit))]
    [HarmonyPrefix]
    internal static bool CalcHitPatch(AttackProcess __instance, ref bool __result)
    {
        if (__instance.CC != null && __instance.CC.HasCondition<ConDeathbringer>())
        {
            __instance.crit = true;
            __result = true;
            return false;
        }
        return true;
    }

    [HarmonyPatch(nameof(AttackProcess.Perform))]
    [HarmonyPrefix]
    internal static bool PerformPatch(AttackProcess __instance, int count, bool hasHit, ref float dmgMulti, ref bool maxRoll, bool subAttack)
    {
        if (__instance.TC.isChara && __instance.CC.Evalue(Constants.FeatHermit) > 0)
        {
            // Hermits - When the target is afflicted with Sleep/Paralyze/Faint Conditions, guarantees crits.
            if (__instance.TC.Chara.HasCondition<ConSleep>() || __instance.TC.Chara.HasCondition<ConParalyze>() || __instance.TC.Chara.HasCondition<ConFaint>())
            {
                maxRoll = true;
                dmgMulti += 0.1F;
            }
            
            // Hermits - If you have Shadow Shroud on, your attack has a 25% chance of revealing you, but your attacks will also do additional damage.
            ConShadowShroud shrouded = __instance.CC.GetCondition<ConShadowShroud>();
            if (shrouded != null)
            {
                if (EClass.rnd(4) == 0) shrouded.Kill();
                dmgMulti += .25F;
            }
        }
        return true;
    }
}
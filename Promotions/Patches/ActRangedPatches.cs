using System;
using System.Runtime;
using HarmonyLib;
using PromotionMod.Common;
using PromotionMod.Elements.PromotionFeats;
using PromotionMod.Stats;
using PromotionMod.Stats.Sentinel;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(ActRanged))]
public class ActRangedPatches
{
    [HarmonyPatch(nameof(ActRanged.CanPerform))]
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
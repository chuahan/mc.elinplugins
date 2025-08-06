using HarmonyLib;
using PromotionMod.Stats.Hermit;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(AttackProcess))]
public class AttackProcessPatches
{
    [HarmonyPatch(nameof(AttackProcess))]
    [HarmonyPrefix]
    internal static bool CalcHitAssassinate(AttackProcess __instance, ref bool __result)
    {
        if (__instance.CC != null && __instance.CC.HasCondition<ConDeathbringer>())
        {
            __instance.crit = true;
            __result = true;
            return false;
        }
        return true;
    }
}
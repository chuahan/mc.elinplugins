using HarmonyLib;
using PromotionMod.Stats;
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
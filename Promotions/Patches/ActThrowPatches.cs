using HarmonyLib;
using PromotionMod.Stats;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(ActThrow))]
public class ActThrowPatches
{
    [HarmonyPatch(nameof(ActThrow.CanPerform))]
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
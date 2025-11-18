using HarmonyLib;
using PromotionMod.Stats;
using PromotionMod.Stats.Sharpshooter;
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

    [HarmonyPatch(nameof(ActRanged.TryReload))]
    [HarmonyPostfix]
    public static void TryReloadPostfix(ActRanged __instance, Thing weapon, Thing ammo)
    {
        // For Overwatch and Heavyarm Stances, instantly remove the reload condition.
        if ((Act.CC.HasCondition<StanceOverwatch>() || Act.CC.HasCondition<StanceHeavyarms>()) && Act.CC.HasCondition<ConReload>())
        {
            Act.CC.RemoveCondition<ConReload>();
        }
    }
}
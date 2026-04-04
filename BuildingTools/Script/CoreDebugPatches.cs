using HarmonyLib;
namespace PromotionMod.Patches;

/// <summary>
/// Sets debug container mats to 10k.
/// </summary>
[HarmonyPatch(typeof(CoreDebug))]
public class CoreDebugPatches
{
    [HarmonyPatch(nameof(CoreDebug.SetStartStockpile))]
    [HarmonyPrefix]
    internal static bool SetStartStockpilePatch(CoreDebug __instance, Thing container, ref int num)
    {
        num = 10000;
        return true;
    }
}
using HarmonyLib;
namespace PromotionMod.Patches;

/// <summary>
/// Sets debug container mats to 10k.
/// </summary>
[HarmonyPatch(typeof(SourceData))]
public class SourceLoadingPatches
{
    [HarmonyPatch(nameof(SourceData.Init))]
    [HarmonyPrefix]
    internal static bool SourceMaterialDebugging_InitPatch(SourceMaterial __instance)
    {
        if (__instance.name == "SourceMaterial")
        {
            var testing = __instance.map;
            return true;
        }
        
        return true;
    }
}
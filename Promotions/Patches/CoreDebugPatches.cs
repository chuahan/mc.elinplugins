using HarmonyLib;
using UnityEngine;
namespace PromotionMod.Patches;

// TODO: Delete move this to my own building mod.
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
using HarmonyLib;
using UnityEngine;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(CardRenderer))]
public class CardRendererPatches
{
    [HarmonyPatch(nameof(CardRenderer.Draw), typeof(RenderParam), typeof(Vector3), typeof(bool))]
    [HarmonyPostfix]
    private static void MakeMeShinyPatch(CardRenderer __instance)
    {
        if (__instance.isChara && __instance.owner.IsPCParty)
        {
            __instance.AddExtra("c_unique_evolved");
        }
    }
}
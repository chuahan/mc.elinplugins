using HarmonyLib;
using PromotionMod.Elements.PromotionAbilities.Artificer;
using PromotionMod.Trait.Artificer;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(HotItemHeld))]
public class HotItemHeldPatches
{
    [HarmonyPatch(nameof(HotItemHeld.GetAct))]
    [HarmonyPrefix]
    internal static bool HotItemHeld_ArtificerToolPatch(HotItemHeld __instance, ref Act __result)
    {
        if (__instance.thing.trait is TraitArtificerTool)
        {
            __result = new ActArtificerTool();
            return false;
        }

        return true;
    }
}
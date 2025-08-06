using System.Collections;
using HarmonyLib;
using PromotionMod.Common;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(ConMiasma))]
public class ThingGenPatches
{
    [HarmonyPatch(nameof(ThingGen.Create))]
    [HarmonyPostfix]
    internal static void ArtificerDoubleCrystals(ref Thing __result, string id)
    {
        if (EClass.pc.Evalue(Constants.FeatArtificer) > 0 && (__result.id == "crystal_earth" || __result.id == "crystal_sun" || __result.id == "crystal_mana"))
        {
            __result.Num *= 2;
        }
    }
}
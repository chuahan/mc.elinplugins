using HarmonyLib;
using PromotionMod.Common;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(ThingGen))]
public class ThingGenPatches
{
    [HarmonyPatch(nameof(ThingGen._Create))]
    [HarmonyPostfix]
    internal static void Promotion_ArtificerDoubleCrystalsPatch(ref Thing __result, string id, int idMat, int lv)
    {
        if (EClass.pc != null && __result != null && EClass.pc.Evalue(Constants.FeatArtificer) > 0 && (__result.id == "crystal_earth" || __result.id == "crystal_sun" || __result.id == "crystal_mana"))
        {
            __result.Num *= 2;
        }
    }
}
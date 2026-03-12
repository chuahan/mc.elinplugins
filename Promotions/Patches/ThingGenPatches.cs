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
        if (EClass.pc != null &&
            __result != null &&
            EClass.pc.MatchesPromotion(Constants.FeatArtificer) &&
            __result.id is "crystal_earth" or "crystal_sun" or "crystal_mana")
        {
            __result.Num *= 2;
        }
    }
}
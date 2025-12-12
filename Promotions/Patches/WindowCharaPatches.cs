using Cwl.Helper.Extensions;
using HarmonyLib;
using PromotionMod.Common;
using PromotionMod.Trait;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(WindowChara))]
public class WindowCharaPatches
{
    [HarmonyPatch(nameof(WindowChara.RefreshProfile))]
    [HarmonyPostfix]
    internal static void Promotion_RefreshProfile(WindowChara __instance)
    {
        // If the chara is promoted, change the job name into the promotion with the original job name in parentheses.
        if (__instance.chara != null)
        {
            int promotionId = __instance.chara.GetFlagValue(Constants.PromotionFeatFlag);
            if (promotionId > 0)
            {
                __instance.textBio.text = $"{TraitPromotionManual.PromotionIdToPromotionNameMap[promotionId].lang()} ({__instance.chara.job.GetText().ToTitleCase()})";
            }
        }
    }
}
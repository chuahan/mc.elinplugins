using HarmonyLib;
using PromotionMod.Common;
using PromotionMod.Stats.Battlemage;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(Element))]
public class ElementPatches
{
    [HarmonyPatch(nameof(Element.GetCost))]
    [HarmonyPostfix]
    internal static void GetCostPatch(Element __instance, ref Act.Cost __result, Chara c)
    {
        if (c.MatchesPromotion(Constants.FeatBattlemage) &&
            __instance is Spell &&
            __result.type == Act.CostType.MP)
        {
            // Focus Stance increases costs based on current mana.
            StanceManaFocus focusCon = c.GetCondition<StanceManaFocus>();
            if (focusCon != null && c.mana.value > 0)
            {
                __result.cost += (int)(c.mana.value * 0.15F);
            }
        }
    }
}
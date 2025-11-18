using HarmonyLib;
using PromotionMod.Common;
using PromotionMod.Stats.Battlemage;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(ELEMENT))]
public class ElementPatches
{
    [HarmonyPatch(nameof(Element.GetCost))]
    [HarmonyPostfix]
    internal static void GetCostPatch(Element __instance, ref Act.Cost __result, Chara c)
    {
        if (c.Evalue(Constants.FeatBattlemage) > 0 && __instance is Spell && __result.type == Act.CostType.MP)
        {
            // Focus Stance increases costs based on current mana.
            StanceManaFocus focusCon = c.GetCondition<StanceManaFocus>();
            if (focusCon != null && c.mana.value > 0)
            {
                __result.cost += (int)(c.mana.value * 0.05F);
            }
        }
    }
}
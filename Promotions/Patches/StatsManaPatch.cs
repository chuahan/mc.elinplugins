using HarmonyLib;
using PromotionMod.Stats.Dancer;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(StatsMana))]
public class StatsManaPatch
{
    [HarmonyPatch(nameof(StatsMana.Mod))]
    [HarmonyPrefix]
    internal static bool ManaMod_CostReduction_Patch(ref int a)
    {
        // Energy Dance reduces mana costs by a flat 15%. Cannot reduce it to 0.
        if (BaseStats.CC.HasCondition<ConEnergyDance>() && a < 0 && a > 1)
        {
            a = (int)(a * .85F);
        }
        return true;
    }
}
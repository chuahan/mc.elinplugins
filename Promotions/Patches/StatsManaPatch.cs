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
        // Energy Dance reduces mana costs by a flat 10%, 20% if partner. Cannot reduce it to 0.
        if (BaseStats.CC.HasCondition<ConEnergyDance>() && a < 0 && a > 1)
        {
            ConEnergyDance dance = BaseStats.CC.GetCondition<ConEnergyDance>();
            float reduction = (float)(1 - dance.power * 0.1);
            a = (int)(a * reduction);
        }
        return true;
    }
}
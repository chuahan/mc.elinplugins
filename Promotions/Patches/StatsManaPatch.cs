using HarmonyLib;
using PromotionMod.Stats;
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
        if (BaseStats.CC.HasCondition<ConEnergyDance>() && a < 0)
        {
            ConEnergyDance dance = BaseStats.CC.GetCondition<ConEnergyDance>();
            float reduction = (float)(1 - dance.power * 0.1);
            a = (int)(a * reduction);
        }

        if (BaseStats.CC.HasCondition<ConSpellTempo>() && a < 0)
        {
            // Elementalist - Spell Tempo reduces mana costs by 5% per stack, capping at 50% mana reduction.
            ConSpellTempo tempo = BaseStats.CC.GetCondition<ConSpellTempo>();
            float reduction = 1 - tempo.power * 0.05F;
            a = (int)(a * reduction);
        }
        return true;
    }
}
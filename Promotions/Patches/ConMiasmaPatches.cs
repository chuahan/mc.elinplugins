using HarmonyLib;
using PromotionMod.Common;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(ConMiasma))]
public class ConMiasmaPatches
{
    [HarmonyPatch(nameof(ConMiasma.Tick))]
    [HarmonyPrefix]
    internal static bool HarbingerTick(ConMiasma __instance)
    {
        foreach (Chara ally in HelperFunctions.GetCharasWithinRadius(__instance.owner.pos, 2F, __instance.owner, true, false))
        {
            if (ally.Evalue(Constants.FeatHarbinger) > 0)
            {
                // Harbinger will consume the miasma instead, regaining health.
                int healAmount = (int)(ally.MaxHP * 0.1F);
                ally.HealHP(healAmount);
                __instance.Kill();
                return false;
            }
        }
        return true;
    }
}
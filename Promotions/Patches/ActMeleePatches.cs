using System.Runtime;
using HarmonyLib;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(ActMelee))]
public class ActMeleePatches
{
    [HarmonyPatch(nameof(ActMelee.Attack))]
    [HarmonyPrefix]
    public static bool AttackPatch(ActMelee __instance, ref float dmgMulti, ref bool maxRoll)
    {
        // Berserker - 
        // When Act Melee.
        return true;
    }
}
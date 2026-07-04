using HarmonyLib;
using NierMod.Stats;
namespace NierMod.Patches;

[HarmonyPatch(typeof(Card))]
internal class CardPatches : EClass
{
    [HarmonyPatch(nameof(Card.HealHP))]
    [HarmonyPrefix]
    internal static bool OnHealHP(Card __instance)
    {
        if (__instance.isChara)
        {
            if (__instance.Chara.HasCondition<ConUnfinishedBusiness>())
            {
                __instance.Say("debuffUnfinishedBusiness".lang());
                return false;
            }
        }
        return true;
    }
}
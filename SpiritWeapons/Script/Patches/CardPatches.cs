using HarmonyLib;
namespace SpiritWeapons.Patches;

[HarmonyPatch(typeof(Card))]
internal class CardPatches : EClass
{
    [HarmonyPatch(nameof(Card.MakeEgg))]
    [HarmonyPrefix]
    internal static bool SpiritWeapons_MakeEggPatch(Card __instance)
    {
        if (__instance.isChara && __instance.Chara.trait is TraitSpiritWeaponChara)
        {
            return false;
        }
        return true;
    }
}
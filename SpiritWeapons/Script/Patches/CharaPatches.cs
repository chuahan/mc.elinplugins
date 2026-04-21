using System.Collections.Generic;
using HarmonyLib;
namespace SpiritWeapons.Patches;

[HarmonyPatch(typeof(Chara))]
internal class CharaPatches : EClass
{
    [HarmonyPatch(nameof(Chara.MakeGene))]
    [HarmonyPrefix]
    internal static bool SpiritWeapons_MakeGenePatch(Chara __instance)
    {
        if (__instance.trait is TraitSpiritWeaponChara)
        {
            return false;
        }
        return true;
    }

    [HarmonyPatch(nameof(Chara.TickConditions))]
    [HarmonyPrefix]
    internal static bool SpiritWeapons_TickConditionsPatch(Chara __instance)
    {
        // This is used by the original function to regulate the rate of these calculations.
        int turnMod = (__instance.turn + 1) % 50;

        if (turnMod == 1)
        {
            // Tick Spirit Weapons Hunger just like the characters.
            List<Thing> spiritWeapons = __instance.things.FindAll(x => x.IsEquipment && x.HasElement(Common.SpiritWeaponEnc));
            if (spiritWeapons.Count > 0)
            {
                foreach (Thing spiritWeapon in spiritWeapons)
                {
                    spiritWeapon.TickSpiritWeaponHunger();
                }
            }
        }

        return true;
    }
}
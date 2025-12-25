using System.Collections.Generic;
using HarmonyLib;

namespace LivestockTweaks.Patches
{
    [HarmonyPatch(typeof(TraitFoodEggFertilized))]
    internal class TraitFoodEggFertilizedPatches : EClass
    {
        [HarmonyPatch(nameof(TraitFoodEggFertilized.Incubate))]
        [HarmonyPostfix]
        internal static void UpdateBabySettings(ref Chara __result)
        {
            if (LivestockTweaksConfig.StayStill?.Value == true) __result.noMove = true;
            if (LivestockTweaksConfig.UseEquipment?.Value == true) __result.SetInt(113, 1);
        }
    }
}

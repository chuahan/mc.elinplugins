using HarmonyLib;

namespace ScarabShenanigans.Patches
{
    [HarmonyPatch(typeof(Chara))]
    internal class ScarabShenanigansPatches : EClass
    {
        [HarmonyPatch(nameof(Chara.CanDuplicate))]
        [HarmonyPrefix]
        internal static bool CanDuplicatePrefix(Chara __instance, ref bool __result)
        {
            if (__instance.id != "mech_scarab") return true;
            __result = false;
            return false;
        }
    }
}

using HarmonyLib;

namespace PostalOptions;

[HarmonyPatch(typeof(FactionBranch))]
public class FactionBranchPatches
{
    [HarmonyPatch(nameof(FactionBranch.ReceivePackages))]
    [HarmonyPrefix]
    internal static bool PostalOptions_OnlyDeliverHome(FactionBranch __instance)
    {
        if (EClass.player.simulatingZone)
        {
            return false;
        }

        return EClass._zone.uid == EClass.pc.homeZone.uid;

    }
}
using HarmonyLib;

namespace BuildingTools;

/// <summary>
/// Stops garbage from spawning.
/// </summary>
[HarmonyPatch(typeof(FactionBranch))]
public class GenerateGarbagePatches
{
    [HarmonyPatch(nameof(FactionBranch.GenerateGarbage))]
    [HarmonyPrefix]
    internal static bool StopGarbage(FactionBranch __instance)
    {
        return false;
    }
}
using HarmonyLib;

namespace BuildingTools;

/// <summary>
/// Stops enemies from spawning.
/// </summary>
[HarmonyPatch(typeof(Zone))]
public class ZonePatches
{
    [HarmonyPatch(nameof(Zone.SpawnMob), typeof(Point), typeof(SpawnSetting))]
    [HarmonyPrefix]
    internal static bool StopEnemySpawns(Zone __instance, Point pos, SpawnSetting setting)
    {
        if (__instance.IsPCFaction)
        {
            return false;
        }

        return true;
    }
}
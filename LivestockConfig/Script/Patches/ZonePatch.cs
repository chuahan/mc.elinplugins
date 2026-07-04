using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
namespace LivestockTweaks.Patches;

[HarmonyPatch(typeof(Zone))]
internal class ZonePatch
{
    [HarmonyPatch(nameof(Zone.Simulate))]
    [HarmonyPostfix]
    internal static void ZoneSimulate_StayOnSign(Zone __instance)
    {
        if (!EClass._map.zone.IsPCFaction) return;
        List<Chara> list = EClass._map.charas.ToList();
        Thing ranchSign = EClass._map.FindThing(typeof(TraitSpotRanch));
        if (ranchSign == null) return;
        if (LivestockTweaksConfig.CompactOnRanch?.Value == false) return;
        foreach (Chara chara in list.Where(chara => chara.memberType == FactionMemberType.Livestock))
        {
            chara.MoveImmediate(ranchSign.pos);
        }
    }
}

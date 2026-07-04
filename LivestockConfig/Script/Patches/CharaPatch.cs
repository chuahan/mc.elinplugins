using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
namespace LivestockTweaks.Patches;

[HarmonyPatch(typeof(Chara))]
internal class CharaPatch
{

    [HarmonyPatch(nameof(Chara._Move))]
    [HarmonyPrefix]
    internal static bool Chara_OnMovePatch(Chara __instance, ref Card.MoveResult __result, Point newPoint, Card.MoveType type = Card.MoveType.Walk)
    {
        if (!EClass._map.zone.IsPCFaction) return true;
        Thing ranchSign = EClass._map.FindThing(typeof(TraitSpotRanch));
        if (ranchSign == null) return true;
        if (__instance.memberType == FactionMemberType.Livestock)
        {
            if (__instance.pos.Equals(ranchSign.pos) && __instance.noMove)
            {
                __result = Card.MoveResult.Fail;
                return false;
            }
        }

        return true;
    }
}

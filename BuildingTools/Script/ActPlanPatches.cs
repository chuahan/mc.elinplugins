using System.Collections.Generic;
using System.Linq;
using HarmonyLib;

namespace BuildingTools;

/// <summary>
/// This patch allows you to fast recruit allies and clean the entire map of debris.
/// </summary>
[HarmonyPatch(typeof(ActPlan))]
public class ActPlanPatches
{
    [HarmonyPatch(nameof(ActPlan._Update))]
    [HarmonyPostfix]
    internal static void ActPlanUpdatePatch(ActPlan __instance, PointTarget target)
    {
        if (EClass._zone != null && EClass._zone.IsPCFaction && __instance.input == ActInput.AllAction)
        {
            Point targetPoint = target.pos;
            foreach (Chara c in targetPoint.Charas)
            {
                if (c is { IsPCFaction: false })
                {
                    __instance.TrySetAct($"Make NPC {c.NameSimple}", delegate
                    {
                        c.ModAffinity(EClass.pc, 75);
                        c.noMove = true;
                        //EMono.Branch.Recruit(c);
                        EClass._zone.branch.AddMemeber(c);
                        return true;
                    });
                }
            }

            __instance.TrySetAct("Clean Map Debris", delegate
            {
                EClass._zone.map.ForeachPoint(CleanAction);

                return true;
            });
            
            __instance.TrySetAct("PURGE HERETICS", delegate
            {
                List<Chara> hostilesInMap = EClass._zone.map.charas.Where(chara => !chara.IsPCFactionOrMinion).ToList();
                foreach (Chara c in hostilesInMap)
                {
                    c.Die();
                    EClass._zone.map._RemoveCard(c);
                }
                return true;
            });
        }
        

    }
    private static void CleanAction(Point pos)
    {
        EClass._map.SetDecal(pos.x, pos.z);
        EClass._map.SetLiquid(pos.x, pos.z, 0, 0);
    }
}
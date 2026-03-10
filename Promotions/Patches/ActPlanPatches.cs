using System.Collections.Generic;
using HarmonyLib;
using PromotionMod.Common;
using UnityEngine;
namespace PromotionMod.Patches;

// TODO: Delete move this to my own building mod.
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
        }
        
        if (__instance.input == ActInput.AllAction)
        {
            Point targetPoint = target.pos;
            foreach (Chara c in targetPoint.Charas)
            {
                __instance.TrySetAct($"Debugging", delegate
                {
                    var abilityCon = c.ability;
                    return true;
                });
            }
        }
    }
}
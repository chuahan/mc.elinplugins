using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using UnityEngine;
namespace Evolution.Patches;

[HarmonyPatch(typeof(DramaCustomSequence))]
public class DramaCustomSequencePatches
{
    [HarmonyPatch(nameof(DramaCustomSequence.Build))]
    [HarmonyPostfix]
    internal static void CanEvolvePatch(DramaCustomSequence __instance, Chara c)
    {
        if (c is { isChara: true, IsPCFaction: true, IsMinion: false })
        {
            (bool evolvable, string charaResult, string evoThing) = c.IsEvolvable();
            if (evolvable)
            {
                var choice = new DramaChoice("daEvolve".lang(c.NameSimple, evoThing, charaResult), "evolution");
                __instance.manager.lastTalk.choices.Add(choice);
                __instance.manager._choices.Add(choice);
            }
        }
    }
}
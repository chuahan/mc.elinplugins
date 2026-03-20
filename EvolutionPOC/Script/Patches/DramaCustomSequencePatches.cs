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
                __instance.Choice("daEvolve".lang(c.NameSimple, evoThing, charaResult), "_evolve");   
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using BardMod.Common;
using BardMod.Common.HelperFunctions;
using BardMod.Stats;
using BardMod.Stats.BardSongConditions;
using HarmonyLib;

namespace BardMod.Patches;

[HarmonyPatch(typeof(Chara))]
internal class BardCharaPatches : EClass
{
 
    [HarmonyPatch(nameof(Chara.Die))]
    [HarmonyPrefix]
    internal static bool OnDie(Chara __instance, Element e, Card origin, AttackSource attackSource)
    {
        // Ephemeral Flames like Fire
        // If the owner dies while under the effect of brittle. Explode and deal % HP damage to nearby friendlies.
        if (__instance.HasCondition<ConEphemeralFlowersSong>())
        {
            ConEphemeralFlowersSong ephemeralFlowers = __instance.GetCondition<ConEphemeralFlowersSong>();
            List<Chara> targets = HelperFunctions.GetCharasWithinRadius(__instance.pos, 3f, ephemeralFlowers.Caster, false, false);
            int damage = __instance.MaxHP * (ephemeralFlowers.GetHpPercentDamage() / 100);
            // TODO: Play FX
            // TODO: Play SFX
            foreach (Chara target in targets)
            {
                target.AddCondition<ConEphemeralFlowersSong>(ephemeralFlowers.power);
                target.AddCondition<ConFreeze>(30, true);
                target.DamageHP(damage, Constants.EleCold, 100, AttackSource.Shockwave);
            }
        }

        return true;
    }
    
    [HarmonyPatch(nameof(Chara.CalcCastingChance))]
    [HarmonyPostfix]
    internal static void CastingWithCharmed(Chara __instance, ref int __result)
    {
        // Charmed and Disruption both inflict -25% cast chance.
        if (__instance.Chara.HasCondition<ConCharmed>())
        {
            __result = Math.Max(1, __result - 25);   
        }
        
        if (__instance.Chara.HasCondition<ConDisruptionSong>())
        {
            __result = Math.Max(1, __result - 25);
        }
    }
}
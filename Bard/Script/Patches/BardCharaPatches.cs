using System;
using System.Collections.Generic;
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
        // If Kumi Blessed: Plantlife spawns.
        if (__instance.HasCondition<ConEphemeralFlowersSong>())
        {
            ConEphemeralFlowersSong ephemeralFlowers = __instance.GetCondition<ConEphemeralFlowersSong>();
            List<Chara> targets = HelperFunctions.GetCharasWithinRadius(__instance.pos, 3f, ephemeralFlowers.Caster, false, false);
            int damage = __instance.MaxHP * (ephemeralFlowers.GetHpPercentDamage() / 100);
            // TODO: Play FX
            // TODO: Play SFX
            foreach (Chara target in targets)
            {
                ConEphemeralFlowersSong recursive =
                        ConBardSong.Create(nameof(ConEphemeralFlowersSong), ephemeralFlowers.power, ephemeralFlowers.RhythmStacks, ephemeralFlowers.GodBlessed,
                            ephemeralFlowers.Caster) as ConEphemeralFlowersSong;
                target.AddCondition(recursive);
                target.AddCondition<ConFreeze>(30, true);
                // target.DamageHP(dmg: damage, ele: Constants.EleCold, eleP: 100, attackSource: AttackSource.Shockwave);
                BardCardPatches.CachedInvoker.Invoke(
                    target,
                    new object[] { damage, Constants.EleCold, 100, AttackSource.Shockwave, null }
                );
            }

            if (ephemeralFlowers.GodBlessed)
            {
                Chara newPlant = CharaGen.CreateFromFilter("c_plant", __instance.LV);
                _zone.AddCard(newPlant, __instance.pos.Copy());
                if (newPlant.LV < __instance.LV)
                {
                    newPlant.SetLv(__instance.LV);
                }
                newPlant.MakeMinion(origin.IsPCParty || origin.IsPCPartyMinion ? pc : origin.Chara, MinionType.Friend);
                Msg.Say("plant_pop", __instance, newPlant);
            }
        }

        // Heavens Fall Song - Increase duration on kill if Godblessed.
        if (origin != null && origin.isChara && origin.HasCondition<ConHeavensFallSong>())
        {
            ConHeavensFallSong heavensFall = origin.Chara.GetCondition<ConHeavensFallSong>();
            if (heavensFall.GodBlessed)
            {
                heavensFall.Mod(1);
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
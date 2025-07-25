﻿using BepInEx;
using HarmonyLib;

namespace BardMod
{
    /*
     * Todo List:
     * ===== CHANGE IDS to > 60k. I choose 89,000 as my range.
     * 890xx - 89099 = Characters
     * 89100 - 89499 = Elements
     * 89100 - Finale
     * 891xx - Basic Verses
     * 892xx - Choruses
     * 893xx - Instrument Mods
     * 894xx - Feats and NPC abilities.
     * 89500 - 89999 = Stats
     * ===== Overall:
     * Check bard power. It seems too weak, though my character might just suck.
     * Consider changing restoration songs (Mystic/Healing/LonelyTears) to instead heal on tick based, or once every X ticks.
     * Add FX for all songs. Echo Slam is the only one with SFX FX right now. Add a new FX for applying song buffs.
     * Fix the log message for all songs. Give them color somehow to make them pop out more. How tf do I do that?
     * ===== Per Song:
     * rise_and_roar- Good
     * dance_and_dash- Good
     * vim_and_vitalize - Maybe Change to delayed tick buff? Seems too slow. Just a worse HOT early and better one Late. Still not used Late.
     * weave_and_wield- Good
     * shield_and_shelter - Add scaling to overguard amount. 800 at void 500 means nothing. Test character gets like 80k overguard tho.
     * diminish_and_dread- Good
     * shatter_and_scatter - Good
     * distort_and_daze- Good
     * whisper_and_wither- Good
     * charm_and_cradle- Good
     * chorus_purity - Good
     * chorus_slash- Good
     * chorus_shake- Good
     * chorus_echoes - Good
     * hollow_symphony- Good
     * wind_goddess- Good
     * alluring_dance- Good
     * clear_thunder - Good
     * farewell_flames - Good
     * endless_blossoms - Good
     * unshaking_earth - Good
     * lonely_tears - Good
     * abyss_reflection - Good
     * prismatic_bridge - Good
     * shimmering_dew - Good
     * heavens_fall - Good
     * after_tempest - Good
     * shooting_stars - Good, seems kind of weak. Get feedback.
     * ephemeral_flowers - Good
     * moonlit_flight - Good
     * ===== New Songs Ideas:
     * Non combat songs
     * - Plant Growth song sounds too overpowered? Unless I scale cost based on tiles affected.
     * - Mining spell to destroy rocks in AOE. Has to take in your existing mining skill + tool though.
     * - Brown note, except egg laying? Naw sounds too silly.
     * - Dancing lights - Creates multiple lights that shine light and reveal invisibility.
     * - Disguise song - Incognito effect.
     * - Summon Songbird - Summons a non combat minion with duet. Also boosts performance score during music quests. Chance of summoning berserk swole songbird that punches enemies.
     * - Dousing Rain - Calls for the rain.
     * - Bar Song - Causes nearby characters to all get drunk.
     * Combat Songs
     * - Invisibility Spell.
     * - Dragonbreath Voice / Spit Fire - Ignites nearby enemies, Fire Sunder.
     */
    internal static class ModInfo
    {
        internal const string Guid = "mc.elinplugins.bard";
        internal const string Name = "BardMagic";
        internal const string Version = "1.0.0";
    }

    [BepInPlugin(ModInfo.Guid, ModInfo.Name, ModInfo.Version)]
    internal class BardMod : BaseUnityPlugin
    {
        internal static BardMod? Instance { get; private set; }

        internal const bool Debug = false;

        private void Awake()
        {
            Instance = this;
            var harmony = new Harmony(ModInfo.Guid);

            BardConfig.Load(Config);
            
            harmony.PatchAll();
        }

        internal static void Log(object payload)
        {
            Instance!.Logger.LogInfo(payload);
        }
    }
}
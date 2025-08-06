using BardMod.Patches;
using BepInEx;
using HarmonyLib;
using HarmonyLib.Public.Patching;
namespace BardMod;

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
 * vim_and_vitalize - Maybe Change to delayed tick buff? Seems too slow. Just a worse HOT early and better one Late. Still not used Late.
 * shield_and_shelter - Add scaling to overguard amount. 800 at void 500 means nothing. Test character gets like 80k overguard tho.
 * shooting_stars - Good, seems kind of weak. Get feedback.
 * ===== New Songs Ideas:
 * Non combat songs
 * - Plant Growth song sounds too overpowered? Unless I scale cost based on tiles affected.
 * - Toss a Coin to your witcher.
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
    internal const string Guid = "cantus.bard.mod";
    internal const string Name = "BardMagic";
    internal const string Version = "1.0.1";
}

[BepInPlugin(ModInfo.Guid, ModInfo.Name, ModInfo.Version)]
internal class BardMod : BaseUnityPlugin
{

    internal const bool Debug = false;
    internal static BardMod? Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        Harmony harmony = new Harmony(ModInfo.Guid);

        BardConfig.Load(Config);

        harmony.PatchAll();
    }

    internal static void Log(object payload)
    {
        Instance!.Logger.LogInfo(payload);
    }
}
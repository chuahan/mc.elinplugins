using HarmonyLib;
using PromotionMod.Common;
using PromotionMod.Source;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(FactionManager))]
public class FactionManagerPatch
{
    [HarmonyPatch(nameof(FactionManager.OnLoad))]
    [HarmonyPrefix]
    internal static bool FactionManager_OnLoad_LoadNewFactions(FactionManager __instance)
    {
        // Factions are only created when a new game is created. So existing files will not have access to these new factions.
        // This patch will fix the existing factions to include the three of them from the Promotion Mod.
        if (__instance.Find(Constants.AluenaFactionId) == null)
        {
            SourceFaction.Row factionRow = EClass.sources.factions.GetRow(Constants.AluenaFactionId);
            Faction.Create(factionRow);
        }
        
        if (__instance.Find(Constants.AdvGuildFactionId) == null)
        {
            SourceFaction.Row factionRow = EClass.sources.factions.GetRow(Constants.AdvGuildFactionId);
            Faction faction = new GuildAdventurer
            {
              id = factionRow.id,
              _source = factionRow,
              
            };
            faction.Init();

        }
        
        if (__instance.Find(Constants.InfoGuildFactionId) == null)
        {
            SourceFaction.Row factionRow = EClass.sources.factions.GetRow(Constants.InfoGuildFactionId);
            Faction faction = new GuildInformation
            {
                id = factionRow.id,
                _source = factionRow,
              
            };
            faction.Init();
        }

        // Sanity Check
        return true;
    }
}
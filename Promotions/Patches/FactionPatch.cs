using HarmonyLib;
using PromotionMod.Common;
using PromotionMod.Source;
namespace PromotionMod.Patches;

[HarmonyPatch(typeof(Faction))]
public class FactionPatch
{
    [HarmonyPatch(nameof(Faction.Create))]
    [HarmonyPrefix]
    internal static bool Faction_CreateFix(Faction __instance, ref Faction __result, SourceFaction.Row r)
    {
        // Faction Creation only looks for Elin-classes, which is a big problem for anyone loading in their own Guilds.
        // This overrides the creation to just create my class directly for these two guilds.
        // Aluèna is fine because it's a plain Faction.
        if (r.id is Constants.AdvGuildFactionId)
        {
            Faction faction = new GuildAdventurer
            {
                id = r.id,
                _source = r
            };
            faction.Init();
            __result = faction;
            return false;
        }

        if (r.id is Constants.InfoGuildFactionId)
        {
            Faction faction = new GuildInformation
            {
                id = r.id,
                _source = r
            };
            faction.Init();
            __result = faction;
            return false;
        }

        return true;
    }
}
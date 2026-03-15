namespace PromotionMod.Source;
/*
public class CustomGuild : Guild, IChunkable
{
    internal static readonly Dictionary<string, CustomGuild> Managed = [];

    [JsonProperty] private string _id = "";

    private const string SaveDataChunkName = "PromotionModSaveData.Guild";


    [CwlPostSave]
    internal static void SaveCustomGuild(GameIOProcessor.GameIOContext context)
    {
        Dictionary<string, CustomGuild> guilds = game.factions.dictAll.Values
                .OfType<CustomGuild>()
                .ToDictionary(r => r.id);
        context.Save(guilds, "customGuilds");
    }

    [CwlPostLoad]
    internal static void LoadCustomReligion(GameIOProcessor.GameIOContext context)
    {
        if (!context.Load<Dictionary<string, CustomGuild>>(out Dictionary<string, CustomGuild>? guilds, "customGuilds")) {
            return;
        }

        foreach (var custom in game.religions.list.OfType<CustomGuild>()) {
            if (!religions.TryGetValue(custom.id, out var loaded)) {
                continue;
            }

            custom.giftRank = loaded.giftRank;
            custom.mood = loaded.mood;
            custom.relation = loaded.relation;
        }
    }
}
*/
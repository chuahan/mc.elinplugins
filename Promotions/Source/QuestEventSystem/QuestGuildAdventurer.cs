using PromotionMod.Common;
namespace PromotionMod.Source.QuestEventSystem;

public class QuestGuildAdventurer : QuestGuild
{
    public override Guild guild => game.factions.Find(Constants.AdvGuildFactionId) as Guild;
}
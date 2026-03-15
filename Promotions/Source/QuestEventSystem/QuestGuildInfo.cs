using PromotionMod.Common;
namespace PromotionMod.Source.QuestEventSystem;

public class QuestGuildInfo : QuestGuild
{
    public override Guild guild => game.factions.Find(Constants.InfoGuildFactionId) as Guild;
}
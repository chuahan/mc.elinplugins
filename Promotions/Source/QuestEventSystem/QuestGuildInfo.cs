using PromotionMod.Common;
namespace PromotionMod.Source.QuestEventSystem;

public class QuestGuildInfo : QuestGuild
{
    public override Guild guild => EClass.game.factions.Find(Constants.InfoGuildFactionId) as Guild;
}
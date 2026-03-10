using PromotionMod.Common;
namespace PromotionMod.Source.QuestEventSystem;

public class QuestGuildAdventurer : QuestGuild
{
    public override Guild guild => EClass.game.factions.Find(Constants.AdvGuildFactionId) as Guild;
}
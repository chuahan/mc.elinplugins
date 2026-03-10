using PromotionMod.Source.QuestEventSystem;
namespace PromotionMod.Source;

public class GuildInformation : Guild
{
    //public override QuestGuild Quest => EClass.game.quests.Get<QuestGuildInfo>();
    public override bool IsCurrentZone => EClass._zone.id == "aluena";
}
namespace PromotionMod.Source;

public class GuildInformation : Guild
{
    //public override QuestGuild Quest => EClass.game.quests.Get<QuestGuildInfo>();
    public override bool IsCurrentZone => _zone.id == "aluena";
}
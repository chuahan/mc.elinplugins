namespace PromotionMod.Source;

public class GuildAdventurer : Guild
{
    //public override QuestGuild Quest => EClass.game.quests.Get<QuestGuildAdventurer>();
    public override bool IsCurrentZone => _zone.id == "aluena";
}
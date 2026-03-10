using PromotionMod.Source.QuestEventSystem;
namespace PromotionMod.Source;

public class GuildAdventurer : Guild
{
    //public override QuestGuild Quest => EClass.game.quests.Get<QuestGuildAdventurer>();
    public override bool IsCurrentZone => EClass._zone.id == "aluena";
}
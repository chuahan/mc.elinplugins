using PromotionMod.Source;
namespace PromotionMod.Trait.Characters;

public class TraitVyers : TraitUniqueCharaNoJoin
{
    public override bool CanGiveRandomQuest => false;
    
    public override int RestockDay => 7;

    public void _OnBarter()
    {
        Thing merchantInventory = owner.things.Find("chest_merchant");
        if (merchantInventory == null)
        {
            merchantInventory = ThingGen.Create("chest_merchant");
            owner.AddThing(merchantInventory);
        }
        merchantInventory.c_lockLv = 0;

        merchantInventory.things.DestroyAll(_t => _t.GetInt(101) != 0);

        foreach (Thing oldStock in merchantInventory.things)
        {
            oldStock.invX = -1;
        }

        GuildAdventurer advGuild = EClass.game.factions.Find<GuildAdventurer>("guild_adventurer_aluena");
        // At Max Rank. Player can buy Advanced Combat Skillbooks from Vyers.
        if (advGuild.IsMember && advGuild.relation.rank >= 6)
        {
            // Add a random Advanced Combat Manual
            merchantInventory.AddCard(ThingGen.Create("book_combat_skill"));
            // Add 5 Advanced Combat Skill Forgetting Manuals
            merchantInventory.AddCard(ThingGen.Create("book_combat_forget").SetNum(5));
        }
    }
}
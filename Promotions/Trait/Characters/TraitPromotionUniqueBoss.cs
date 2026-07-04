namespace PromotionMod.Trait.Characters;

// Unique Bosses are unrecruitable.
// Unique Bosses on defeat will mark their defeat in the player log, preventing them from spawning again.
public class TraitPromotionUniqueBoss : TraitPromotionUnrecruitable
{
    public override bool RecruitmentCondition => false;
    public override bool CanBout => false;
    public override bool CanWhore => false;
    public override bool CanGiveRandomQuest => false;
    
}
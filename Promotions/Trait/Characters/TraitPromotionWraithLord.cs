namespace PromotionMod.Trait.Characters;

// Wraith Lords are slightly different from normal Unique Bosses.
// While they CAN be defeated, they will return to life if a certain amount of time passes, unless Morgoth is slain.
public class TraitPromotionWraithLord : TraitPromotionUniqueBoss
{
    public override bool RecruitmentCondition => false;
    public override bool CanBout => false;
    public override bool CanWhore => false;
    public override bool CanGiveRandomQuest => false;
}
using PromotionMod.Common;
namespace PromotionMod.Trait.Characters;

public class TraitWatcherKnight : TraitPromotionUniqueCharacter
{
    public override bool CanGiveRandomQuest => false;

    public override int Prepromotion => Constants.FeatHolyKnight;
}
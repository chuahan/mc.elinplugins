using PromotionMod.Common;
namespace PromotionMod.Trait.Characters;

public class TraitMinari : TraitPromotionUniqueCharacter
{
    public override bool CanGiveRandomQuest => false;
    
    public override int Prepromotion => Constants.FeatTrickster;
}
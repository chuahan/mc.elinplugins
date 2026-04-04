using PromotionMod.Common;
namespace PromotionMod.Trait.Characters;

public class TraitVessia : TraitPromotionUniqueCharacter
{
    public override bool IsBefriendedThroughDialog => true;

    public override int Prepromotion => Constants.FeatDancer;
    public override int RestockDay => 5;
}
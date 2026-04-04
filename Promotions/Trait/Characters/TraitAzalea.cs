using PromotionMod.Common;
namespace PromotionMod.Trait.Characters;

public class TraitAzalea : TraitPromotionUniqueCharacter
{
    public override bool IsBefriendedThroughDialog => true;

    public override int Prepromotion => Constants.FeatDruid;
    public override int RestockDay => 5;
}
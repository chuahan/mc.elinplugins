using PromotionMod.Common;
namespace PromotionMod.Trait.Characters;

public class TraitLouise : TraitPromotionUniqueCharacter
{
    public override bool IsBefriendedThroughDialog => player.dialogFlags.TryGetValue("louiseRecruited") > 0;

    public override int Prepromotion => Constants.FeatSniper;
}
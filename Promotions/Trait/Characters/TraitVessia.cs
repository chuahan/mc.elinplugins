using PromotionMod.Common;
namespace PromotionMod.Trait.Characters;

public class TraitVessia : TraitPromotionUniqueCharacter
{
    public override bool IsBefriendedThroughDialog => player.dialogFlags.TryGetValue("vessiaRecruited") > 0;

    public override int Prepromotion => Constants.FeatDancer;
}
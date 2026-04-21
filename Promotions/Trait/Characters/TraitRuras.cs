using PromotionMod.Common;
namespace PromotionMod.Trait.Characters;

public class TraitRuras : TraitPromotionUniqueCharacter
{
    public override bool IsBefriendedThroughDialog => player.dialogFlags.TryGetValue("rurasRecruited") > 0;

    public override int Prepromotion => Constants.FeatHermit;
}
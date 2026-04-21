using PromotionMod.Common;
namespace PromotionMod.Trait.Characters;

public class TraitEvie : TraitPromotionUniqueCharacter
{
    public override bool IsBefriendedThroughDialog => player.dialogFlags.TryGetValue("evieRecruited") > 0;

    public override int Prepromotion => Constants.FeatGambler;
}
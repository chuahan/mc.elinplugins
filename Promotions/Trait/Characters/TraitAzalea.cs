using PromotionMod.Common;
namespace PromotionMod.Trait.Characters;

public class TraitAzalea : TraitPromotionUniqueCharacter
{
    public override bool IsBefriendedThroughDialog => player.dialogFlags.TryGetValue("azaleaRecruited") > 0;

    public override int Prepromotion => Constants.FeatDruid;
}
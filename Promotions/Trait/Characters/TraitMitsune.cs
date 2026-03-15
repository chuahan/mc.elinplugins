using PromotionMod.Common;
namespace PromotionMod.Trait.Characters;

public class TraitMitsune : TraitPromotionUniqueCharacter
{
    public override bool IsBefriendedThroughDialog => player.dialogFlags.TryGetValue("mitsuneRecruited") > 0;

    public override int Prepromotion => Constants.FeatDreadKnight;
}
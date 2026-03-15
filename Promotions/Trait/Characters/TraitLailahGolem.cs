using PromotionMod.Common;
namespace PromotionMod.Trait.Characters;

public class TraitLailahGolem : TraitPromotionUniqueCharacter
{
    public override bool IsBefriendedThroughDialog => player.dialogFlags.TryGetValue("lailahGolemRecruited") > 0;

    public override int Prepromotion => Constants.FeatSharpshooter;
}
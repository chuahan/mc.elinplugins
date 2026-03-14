using PromotionMod.Common;
namespace PromotionMod.Trait.Characters;

public class TraitSena : TraitPromotionUniqueCharacter
{
    //public override bool IsBefriendedThroughDialog => player.dialogFlags.TryGetValue("senaRecruited") > 0;
    
    public override int Prepromotion => Constants.FeatHermit;
}
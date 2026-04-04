using PromotionMod.Common;
namespace PromotionMod.Trait.Characters;

public class TraitWatcherSoldier : TraitPromotionUniqueCharacter
{
    public override bool IsBefriendedThroughDialog => true;
    
    public override bool CanGiveRandomQuest => false;
}
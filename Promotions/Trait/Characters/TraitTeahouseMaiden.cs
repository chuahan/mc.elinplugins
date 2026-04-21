namespace PromotionMod.Trait.Characters;

// Camellia cannot be recruited. Someone needs to run the shop.
public class TraitTeahouseMaiden : TraitPromotionUniqueCharacter
{
    public override bool IsBefriendedThroughDialog => false;
}
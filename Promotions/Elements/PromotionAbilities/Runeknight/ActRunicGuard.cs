using PromotionMod.Common;
using PromotionMod.Stats.Runeknight;
namespace PromotionMod.Elements.PromotionAbilities.Runeknight;

public class ActRunicGuard : PromotionSpellAbility
{
    public override int PromotionId => Constants.FeatRuneKnight;
    public override string PromotionString => Constants.RuneKnightId;
    public override int AbilityId => Constants.ActRunicGuardId;

    public override bool Perform()
    {
        // Remove Existing Elemental Attunement.
        ConElementalAttunement existingAttunement = CC.GetCondition<ConElementalAttunement>();
        existingAttunement?.Kill();

        // Apply Runic Guard.
        CC.AddCondition<ConRunicGuard>();
        return true;
    }
}
using PromotionMod.Stats;
namespace PromotionMod.Elements.PromotionAbilities;

/// <summary>
///     A wrapper class to help reduce duplicate code.
/// </summary>
public abstract class PromotionCombatAbility : PromotionAbility
{

    public override PromotionAbilityCostType PromotionAbilityCost => PromotionAbilityCostType.PromotionAbilityCostStamina;

    public override bool CanPerform()
    {
        // Combat type abilities cannot be used while Suppressed or Disabled.
        if (CC.HasCondition<ConSupress>() || CC.HasCondition<ConDisable>()) return false;

        return base.CanPerform();
    }
}
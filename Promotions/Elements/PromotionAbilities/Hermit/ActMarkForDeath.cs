using PromotionMod.Common;
using PromotionMod.Stats;

namespace PromotionMod.Elements;

/// <summary>
///     Hermit Ability
///     Marks an enemy for death.
/// </summary>
public class ActMarkForDeath : PromotionCombatAbility
{
    public override int PromotionId => Constants.FeatHermit;
    public override string PromotionString => Constants.HermitId;
    public override int AbilityId => Constants.ActMarkForDeathId;

    public override PromotionAbilityCostType PromotionAbilityCost => PromotionAbilityCostType.PromotionAbilityCostMana;

    public override bool Perform()
    {
        TC.Chara.AddCondition<ConMarkedForDeath>(force: true);
        return true;
    }
}
using PromotionMod.Common;
using PromotionMod.Stats.Hermit;
namespace PromotionMod.Elements.PromotionAbilities.Hermit;

/// <summary>
///     Hermit Ability
///     Provides advanced stealth.
///     Lifted on Attack.
/// </summary>
public class ActShadowShroud : PromotionCombatAbility
{
    public override int PromotionId => Constants.FeatHermit;
    public override string PromotionString => Constants.HermitId;
    public override int Cooldown => 10;
    public override int AbilityId => Constants.ActShadowShroudId;
    public override PromotionAbilityCostType PromotionAbilityCost => PromotionAbilityCostType.PromotionAbilityCostMana;

    public override bool Perform()
    {
        ConShadowShroud shadowShroud = CC.GetCondition<ConShadowShroud>();
        if (shadowShroud != null)
        {
            shadowShroud.Kill();
            CC.AddCooldown(AbilityId, Cooldown);
        }
        else
        {
            CC.AddCondition<ConShadowShroud>();
        }

        return true;
    }
}
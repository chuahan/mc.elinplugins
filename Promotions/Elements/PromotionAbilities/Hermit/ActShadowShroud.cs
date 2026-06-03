using PromotionMod.Common;
using PromotionMod.Stats;

namespace PromotionMod.Elements;

/// <summary>
///     Hermit Ability
///     Provides advanced stealth.
///     Lifted on Attack.
/// </summary>
public class ActShadowShroud : PromotionCombatAbility
{
    public override int PromotionId => Constants.FeatHermit;
    public override string PromotionString => Constants.HermitId;
    public int Cooldown => 10;
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
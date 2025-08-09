using PromotionMod.Common;
using PromotionMod.Stats.Hermit;
namespace PromotionMod.Elements.PromotionAbilities.Hermit;

/// <summary>
/// Hermit Ability
/// Provides advanced stealth.
/// Lifted on Attack.
/// </summary>
public class ActShadowShroud : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatHermit) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.HermitId.lang()));
            return false;
        }
        return base.CanPerform();
    }

    public override bool Perform()
    {
        CC.AddCondition<ConShadowShroud>();
        return true;
    }
}
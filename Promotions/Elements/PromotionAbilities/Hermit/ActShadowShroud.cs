using PromotionMod.Common;
using PromotionMod.Stats.Hermit;
namespace PromotionMod.Elements.PromotionAbilities.Hermit;

/// <summary>
///     Hermit Ability
///     Provides advanced stealth.
///     Lifted on Attack.
/// </summary>
public class ActShadowShroud : Ability
{
    public override bool CanPerform()
    {
        if (!CC.MatchesPromotion(Constants.FeatHermit))
        {
            Msg.Say("classlocked_ability".lang(Constants.HermitId.lang()));
            return false;
        }
        return base.CanPerform();
    }

    public override Cost GetCost(Chara c)
    {
        Cost convertToMp = base.GetCost(c);
        convertToMp.type = CostType.MP;
        return convertToMp;
    }
    
    public override bool Perform()
    {
        CC.AddCondition<ConShadowShroud>();
        return true;
    }
}
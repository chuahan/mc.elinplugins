using PromotionMod.Common;
using PromotionMod.Stats.Dancer;
namespace PromotionMod.Elements.PromotionAbilities.Dancer;

/// <summary>
///     Dancer Ability
///     PC ONLY
///     Designate an ally as your Dance Partner.
///     You gain the Partner Style Stance.
///     Your buffs no longer affect the rest of your team, only you and your partner with enhanced effects.
/// </summary>
public class ActDancePartner : Ability
{
    public override bool CanPerform()
    {
        if (!CC.IsPC) return false;
        if (CC.Evalue(Constants.FeatDancer) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.DancerId.lang()));
            return false;
        }
        if (TC == null) return false;
        if (TC.Chara != null && !TC.IsPCParty) return false;
        return base.CanPerform();
    }

    public override bool Perform()
    {
        // Remove existing partner.
        if (CC.HasCondition<StancePartnerStyle>())
        {
            StancePartnerStyle existingPartner = CC.GetCondition<StancePartnerStyle>();
            existingPartner.DancePartner.RemoveCondition<ConDancePartner>();
        }

        // Add new partner.
        StancePartnerStyle partnerStance = CC.AddCondition<StancePartnerStyle>() as StancePartnerStyle;
        partnerStance.DancePartner = TC.Chara;
        TC.Chara.AddCondition<ConDancePartner>();
        CC.ShowEmo(Emo.happy);
        TC.ShowEmo(Emo.happy);
        return true;
    }
}
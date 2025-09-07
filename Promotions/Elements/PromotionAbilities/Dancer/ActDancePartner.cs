using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Stats.Dancer;
namespace PromotionMod.Elements.PromotionAbilities.Dancer;

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

        List<Chara> characters = TP.ListCharas();
        if (characters.Count(c => c.IsPCParty || c.IsPC) == 0) return false;
        return base.CanPerform();
    }

    public override bool Perform()
    {
        StancePartnerStyle partnerStance = CC.GetCondition<StancePartnerStyle>();
        List<Chara> potentialPartners = TP.ListCharas();
        potentialPartners.Reverse();

        foreach (Chara chara in potentialPartners)
        {
            // If targeting self while already partner style, switches off Partner Style.
            if (partnerStance != null && chara == CC)
            {
                partnerStance.Kill();
                Msg.Say("dancer_swap_solo".lang(CC.NameSimple));
                return true;
            }

            if (partnerStance == null && chara.IsPCParty && !chara.IsPC)
            {
                StancePartnerStyle partnerStyle = CC.AddCondition<StancePartnerStyle>() as StancePartnerStyle;
                partnerStyle.PartnerUID = TC.Chara.uid;
                CC.ShowEmo(Emo.happy);
                chara.ShowEmo(Emo.happy);
                Msg.Say("dancer_swap_partner".lang(CC.NameSimple, chara.NameSimple));
                return true;
            }
        }

        Msg.Say("dancer_partnerstyle_hint".lang());
        return false;
    }
}
using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Stats.Dancer;
namespace PromotionMod.Elements.PromotionAbilities.Dancer;

public class ActDancePartner : PromotionCombatAbility
{
    public override int PromotionId => Constants.FeatDancer;
    public override string PromotionString => Constants.DancerId;
    public override int AbilityId => Constants.ActDancePartnerId;

    public override bool CanPerformExtra()
    {
        if (!CC.IsPC) return false;

        List<Chara> characters = TP.ListCharas();
        if (characters.Count(c => c.IsPCParty || c.IsPC) == 0) return false;
        return true;
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
                Msg.Say("dancer_swap_solo".langGame(CC.NameSimple));
                return true;
            }

            if (partnerStance == null && chara.IsPCParty && !chara.IsPC)
            {
                StancePartnerStyle partnerStyle = CC.AddCondition<StancePartnerStyle>() as StancePartnerStyle;
                partnerStyle.PartnerUID = TC.Chara.uid;
                CC.ShowEmo(Emo.happy);
                chara.ShowEmo(Emo.happy);
                Msg.Say("dancer_swap_partner".langGame(CC.NameSimple, chara.NameSimple));
                return true;
            }
        }
        return false;
    }
}
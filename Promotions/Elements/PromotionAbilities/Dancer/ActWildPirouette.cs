using PromotionMod.Common;
using PromotionMod.Stats.Dancer;
using UnityEngine.UI;
namespace PromotionMod.Elements.PromotionAbilities.Dancer;

public class ActWildPirouette : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatDancer) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.DancerId.lang()));
            return false;
        }
        return owner?.Chara?.conditions.Exists(x => x is StanceDance) ?? false;
    }

    public override bool Perform()
    {
        bool hasPartner = false;
        Chara partner = null;
        if (CC.HasCondition<StancePartnerStyle>())
        {
            StancePartnerStyle partnerStyle = CC.GetCondition<StancePartnerStyle>();
            partner = _map.FindChara(partnerStyle.PartnerUID);
            hasPartner = true;
        }

        foreach (Chara target in HelperFunctions.GetCharasWithinRadius(CC.pos, 5F, CC, false, true))
        {
            // Attempt to afflict statuses
            if (EClass.rnd(3) == 0)
                ActEffect.ProcAt(EffectId.Debuff, GetPower(Act.CC), BlessedState.Normal, Act.CC, target, target.pos, isNeg: true, new ActRef
                {
                    origin = Act.CC.Chara,
                    n1 = nameof(ConInfatuation),
                });
            if (EClass.rnd(3) == 0) 
                ActEffect.ProcAt(EffectId.Debuff, GetPower(Act.CC), BlessedState.Normal, Act.CC, target, target.pos, isNeg: true, new ActRef
                {
                    origin = Act.CC.Chara,
                    n1 = nameof(ConSleep),
                });
            if (EClass.rnd(3) == 0)
                ActEffect.ProcAt(EffectId.Debuff, GetPower(Act.CC), BlessedState.Normal, Act.CC, target, target.pos, isNeg: true, new ActRef
                {
                    origin = Act.CC.Chara,
                    n1 = nameof(ConJealousy),
                });
            
            // If the Partner is in Melee range, they get a free attack.
            if (hasPartner)
            {
                if (partner.Dist(target) <= partner.body.GetMeleeDistance())
                {
                    new ActMelee().Perform(partner, target);
                }
            }
        }

        return true;
    }
}
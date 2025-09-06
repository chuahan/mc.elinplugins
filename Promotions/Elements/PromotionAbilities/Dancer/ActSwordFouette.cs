using PromotionMod.Common;
using PromotionMod.Stats.Dancer;
namespace PromotionMod.Elements.PromotionAbilities.Dancer;

/// <summary>
/// Dancer Ability
/// 2 Radius Swarm. For each enemy hit it applies a damage reduction to your next blow.
/// </summary>
public class ActSwordFouette : Ability
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
            partner = EClass._map.FindChara(partnerStyle.PartnerUID);
            hasPartner = true;
        }

        foreach (Chara target in HelperFunctions.GetCharasWithinRadius(CC.pos, 2F, CC, false, true))
        {
            target.PlayEffect("ab_swarm");
            target.PlaySound("ab_swarm");
            new ActMeleeSwarm().Perform(CC, target);

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
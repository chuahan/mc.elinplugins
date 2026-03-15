using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Stats.Dancer;
namespace PromotionMod.Elements.PromotionAbilities.Dancer;

/// <summary>
///     Dancer Ability
///     2 Radius Swarm. For each enemy hit it applies a damage reduction to your next blow.
/// </summary>
public class ActSwordFouette : Ability
{
    private float _effectRadius = 2F;

    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatDancer) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.DancerId.lang()));
            return false;
        }

        if (owner?.Chara?.conditions.Exists(x => x is StanceDance) == false)
        {
            if (CC.IsPC) Msg.Say("dancer_mustbedancing".lang());
            return false;
        }

        return base.CanPerform();
    }

    public override void OnMarkMapHighlights()
    {
        List<Point> list = _map.ListPointsInCircle(CC.pos, _effectRadius);
        if (list.Count == 0)
        {
            list.Add(CC.pos.Copy());
        }
        foreach (Point item in list)
        {
            item.SetHighlight(8);
        }
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

        foreach (Chara target in HelperFunctions.GetCharasWithinRadius(CC.pos, _effectRadius, CC, false, true))
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
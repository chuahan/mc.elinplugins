using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Stats.Dancer;
namespace PromotionMod.Elements.PromotionAbilities.Dancer;

public class ActWildPirouette : PromotionCombatAbility
{

    private float _effectRadius = 5F;
    public override int PromotionId => Constants.FeatDancer;
    public override string PromotionString => Constants.DancerId;
    public override int Cooldown => 5;
    public override int AbilityId => Constants.ActWildPirouetteId;

    public override PromotionAbilityCostType PromotionAbilityCost => PromotionAbilityCostType.PromotionAbilityCostMana;

    public override bool CanPerformExtra()
    {
        if (owner?.Chara?.conditions.Exists(x => x is StanceDance) == false)
        {
            if (CC.IsPC) Msg.Say("dancer_mustbedancing".lang());
            return false;
        }

        return true;
    }

    public override Cost GetCost(Chara c)
    {
        Cost convertToMp = base.GetCost(c);
        convertToMp.type = CostType.MP;
        return convertToMp;
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
            // Attempt to afflict statuses
            if (EClass.rnd(3) == 0)
            {
                ActEffect.ProcAt(EffectId.Debuff, GetPower(CC), BlessedState.Normal, CC, target, target.pos, true, new ActRef
                {
                    origin = CC.Chara,
                    n1 = nameof(ConInfatuation)
                });
            }
            if (EClass.rnd(3) == 0)
            {
                ActEffect.ProcAt(EffectId.Debuff, GetPower(CC), BlessedState.Normal, CC, target, target.pos, true, new ActRef
                {
                    origin = CC.Chara,
                    n1 = nameof(ConSleep)
                });
            }
            if (EClass.rnd(3) == 0)
            {
                ActEffect.ProcAt(EffectId.Debuff, GetPower(CC), BlessedState.Normal, CC, target, target.pos, true, new ActRef
                {
                    origin = CC.Chara,
                    n1 = nameof(ConJealousy)
                });
            }

            // If the Partner is in Melee range, they get a free attack.
            if (hasPartner)
            {
                if (partner.Dist(target) <= partner.body.GetMeleeDistance())
                {
                    new ActMelee().Perform(partner, target);
                }
            }
        }

        CC.AddCooldown(AbilityId, Cooldown);

        return true;
    }
}
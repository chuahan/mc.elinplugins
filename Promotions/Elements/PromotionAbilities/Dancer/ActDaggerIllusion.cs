using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Stats.Dancer;
namespace PromotionMod.Elements.PromotionAbilities.Dancer;

public class ActDaggerIllusion : Ability
{
    private const float EffectRadius = 5F;

    private const int MaxTargets = 5;

    public override bool CanPerform()
    {
        if (!CC.MatchesPromotion(Constants.FeatDancer))
        {
            Msg.Say("classlocked_ability".lang(Constants.DancerId.lang()));
            return false;
        }
        Thing thing = ActDaggerIllusion.GetBestThrowingWeapon(CC);
        if (thing == null)
        {
            if (CC.IsPC) Msg.Say("daggerillusion_nothrowingweapon".lang());
            return false;
        }

        if (owner?.Chara?.conditions.Exists(x => x is StanceDance) == false)
        {
            if (CC.IsPC) Msg.Say("dancer_mustbedancing".lang());
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
    
    public override void OnMarkMapHighlights()
    {
        List<Point> list = _map.ListPointsInCircle(CC.pos, EffectRadius);
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

        int targetsHit = 0;
        foreach (Chara target in HelperFunctions.GetCharasWithinRadius(CC.pos, EffectRadius, CC, false, true))
        {
            if (targetsHit >= MaxTargets) break;
            Thing thing = ActDaggerIllusion.GetBestThrowingWeapon(CC);
            if (thing == null)
            {
                return false;
            }
            CC.ranged = thing;
            ActThrow.Throw(CC, target.pos, target, thing.HasElement(410) ? thing : thing.Split(1));
            // Attempt to afflict statuses
            if (EClass.rnd(3) == 0)
            {
                ActEffect.ProcAt(EffectId.Debuff, GetPower(CC), BlessedState.Normal, CC, target, target.pos, true, new ActRef
                {
                    origin = CC.Chara,
                    n1 = nameof(ConParalyze)
                });
            }
            if (EClass.rnd(3) == 0)
            {
                ActEffect.ProcAt(EffectId.Debuff, GetPower(CC), BlessedState.Normal, CC, target, target.pos, true, new ActRef
                {
                    origin = CC.Chara,
                    n1 = nameof(ConBleed)
                });
            }
            if (EClass.rnd(3) == 0)
            {
                ActEffect.ProcAt(EffectId.Debuff, GetPower(CC), BlessedState.Normal, CC, target, target.pos, true, new ActRef
                {
                    origin = CC.Chara,
                    n1 = nameof(ConPoison)
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

            targetsHit++;
        }

        return true;
    }

    public static Thing GetBestThrowingWeapon(Chara cc)
    {
        Thing result = null;
        int equipPower = 0;
        bool returningThrowable = false;

        foreach (Thing thing in cc.things)
        {
            if (thing.HasTag(CTAG.throwWeapon))
            {
                // Prioritize using returning throwables.
                if (returningThrowable && !thing.HasElement(410))
                {
                    continue;
                }

                // Higher equip power
                if (thing.GetEquipValue() > equipPower)
                {
                    result = thing;
                    equipPower = thing.GetEquipValue();
                    returningThrowable = thing.HasElement(410);
                }
            }
        }

        return result;
    }
}
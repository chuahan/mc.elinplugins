using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Stats.Dancer;

public class StanceDance : BaseStance
{
    public float DanceRange = 4f; // SUBJECT TO CHANGE.

    public override void OnStart()
    {
        owner.ShowEmo(Emo.happy);
        for (int i = owner.conditions.Count - 1; i >= 0; i--)
        {
            // Stop any other StanceDance.
            if (owner.conditions[i] is StanceDance dance && dance != this)
            {
                owner.conditions[i].Kill();
                break;
            }
        }
        base.OnStart();
    }

    public virtual void ActInternal(Chara target, int power, bool isPartner)
    {

    }

    public override void Tick()
    {
        bool hasPartner = false;
        Chara partner = null;
        if (CC.HasCondition<StancePartnerStyle>())
        {
            StancePartnerStyle partnerStyle = CC.GetCondition<StancePartnerStyle>();
            if (partnerStyle.DancePartner != null)
            {
                partner = partnerStyle.DancePartner;
                hasPartner = true;
            }
        }

        if (hasPartner)
        {
            if (partner != null && partner.pos.Distance(CC.pos) <= DanceRange)
            {
                // Apply Internal to Dancer and Partner at 1.5x Power and apply alternate effects.
                ActInternal(partner, HelperFunctions.SafeMultiplier(power, 1.5F), true);
                ActInternal(CC, HelperFunctions.SafeMultiplier(power, 1.5F), true);
            }
        }
        else
        {
            List<Chara> affectedAllies = HelperFunctions.GetCharasWithinRadius(CC.pos, DanceRange, CC, true, true);
            foreach (Chara target in affectedAllies)
            {
                ActInternal(target, HelperFunctions.SafeMultiplier(power, 2), false);
            }
        }
    }
}
using PromotionMod.Common;
using PromotionMod.Elements.PromotionFeats;
using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.Sentinel;

public class ActShieldSmite : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatSentinel) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.SentinelId.lang()));
            return false;
        }

        if (CC.body.GetAttackStyle() is not AttackStyle.Shield) return false;
        if (Act.TC == null)
        {
            return false;
        }
        return ACT.Melee.CanPerform();
    }

    public override bool Perform()
    {
        new ActMeleeShieldSmite().Perform(Act.CC, Act.TC);
        return true;
    }
}
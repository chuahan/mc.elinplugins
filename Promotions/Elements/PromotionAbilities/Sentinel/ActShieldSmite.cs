using PromotionMod.Common;
using PromotionMod.Elements.PromotionFeats;
using PromotionMod.Stats;
using PromotionMod.Stats.Sentinel;
using UnityEngine;

namespace PromotionMod.Elements.PromotionAbilities.Sentinel;

public class ActShieldSmite : ActMelee
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatSentinel) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.SentinelId.lang()));
            return false;
        }

        if (CC.body.GetAttackStyle() is not AttackStyle.Shield) return false;
        return base.CanPerform();
    }

    public override bool Perform()
    {
        if (Attack(1f))
        {
            int shieldPower = FeatSentinel.GetShieldPower(CC);
            shieldPower += this.GetPower(CC);
            // TODO: Do I want a multiplier here?
            shieldPower += (int)(TC.MaxHP * .125F);
            HelperFunctions.ProcSpellDamage(this.GetPower(CC), shieldPower, CC, TC.Chara, AttackSource.Melee, ele: Constants.EleVoid);
        }
        return true;
    }
}
using PromotionMod.Common;
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
            int shieldPower = GetShieldDamage(CC);
            shieldPower += this.GetPower(CC);
            // TODO: Do I want a multiplier here?
            shieldPower += (int)(TC.MaxHP * .125F);
            HelperFunctions.ProcSpellDamage(this.GetPower(CC), shieldPower, CC, TC.Chara, AttackSource.Melee, element: Constants.EleVoid);
        }
        return true;
    }

    public static int GetShieldDamage(Chara cc)
    {
        int totalShieldPV = 0;
        foreach (BodySlot slot in cc.body.slots)
        {
            if (slot.elementId == 35 && slot.thing != null && !slot.thing.IsMeleeWeapon)
            {
                totalShieldPV += slot.thing.PV;
            }
        }

        return totalShieldPV;
}
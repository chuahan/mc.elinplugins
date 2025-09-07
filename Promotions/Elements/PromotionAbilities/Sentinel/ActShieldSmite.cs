using PromotionMod.Common;
using PromotionMod.Elements.PromotionFeats;
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
        if (Attack())
        {
            int power = GetPower(CC);
            int shieldPower = FeatSentinel.GetShieldPower(CC);
            shieldPower += power;
            shieldPower += (int)(TC.MaxHP * .125F);
            HelperFunctions.ProcSpellDamage(power, shieldPower, CC, TC.Chara, AttackSource.Melee);
        }
        return true;
    }
}
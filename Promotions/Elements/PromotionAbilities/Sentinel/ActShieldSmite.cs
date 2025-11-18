using PromotionMod.Common;
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
        if (TC == null)
        {
            return false;
        }
        return ACT.Melee.CanPerform();
    }

    public override Cost GetCost(Chara c)
    {
        Cost convertToMp = base.GetCost(c);
        convertToMp.type = CostType.MP;
        return convertToMp;
    }

    public override bool Perform()
    {
        new ActMeleeShieldSmite().Perform(CC, TC);
        return true;
    }
}
using PromotionMod.Common;
using PromotionMod.Stats.Sniper;
namespace PromotionMod.Elements.PromotionAbilities.Sniper;

public class ActTargetHead : PromotionCombatAbility
{
    public override int PromotionId => Constants.FeatSniper;
    public override string PromotionString => Constants.SniperId;
    public override int AbilityId => Constants.ActTargetHeadId;

    public override bool CanPerformExtra()
    {
        if (CC.GetBestRangedWeapon() == null)
        {
            if (CC.IsPC) Msg.Say("sniper_needrangedweapon".langGame());
            return false;
        }
        return ACT.Ranged.CanPerform();
    }

    public override Cost GetCost(Chara c)
    {
        Cost convertToMp = base.GetCost(c);
        convertToMp.type = CostType.MP;
        return convertToMp;
    }

    public override bool Perform()
    {
        ConSniperTarget sniperTarget = CC.AddCondition<ConSniperTarget>(GetPower(CC)) as ConSniperTarget;
        sniperTarget.Target = ConSniperTarget.TargetPart.Head;
        // Perform a Ranged attack at the target.
        Thing rangedWeapon = CC.GetBestRangedWeapon();
        CC.ranged = rangedWeapon;
        ACT.Ranged.Perform(CC, TC);
        sniperTarget.Kill();
        return true;
    }
}
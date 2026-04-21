using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Sniper;

public class ActTacticalRetreat : PromotionCombatAbility
{
    public override int PromotionId => Constants.FeatSniper;
    public override string PromotionString => Constants.SniperId;
    public override int AbilityId => Constants.ActTacticalRetreatId;

    public override bool CanPerformExtra()
    {
        if (CC.GetBestRangedWeapon() == null)
        {
            if (CC.IsPC) Msg.Say("sniper_needrangedweapon".langGame());
            return false;
        }

        return base.CanPerform() && ACT.Ranged.CanPerform();
    }

    public override Cost GetCost(Chara c)
    {
        Cost convertToMp = base.GetCost(c);
        convertToMp.type = CostType.MP;
        return convertToMp;
    }

    public override bool Perform()
    {
        CC.TryMoveFrom(TC.pos);
        // Perform a Ranged attack at the target if you were able to move.
        if (CC.TryMoveFrom(TC.pos) != Card.MoveResult.Fail)
        {
            Thing rangedWeapon = CC.GetBestRangedWeapon();
            CC.ranged = rangedWeapon;
            ACT.Ranged.Perform(CC, TC);
        }

        return true;
    }
}
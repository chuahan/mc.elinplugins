using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Sniper;

public class ActTacticalRetreat : Ability
{
    public override bool CanPerform()
    {
        if (!CC.MatchesPromotion(Constants.FeatSniper))
        {
            Msg.Say("classlocked_ability".lang(Constants.SniperId.lang()));
            return false;
        }

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
using PromotionMod.Common;
using PromotionMod.Stats.Sniper;
namespace PromotionMod.Elements.PromotionAbilities.Sniper;

public class ActTargetHead : Ability
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
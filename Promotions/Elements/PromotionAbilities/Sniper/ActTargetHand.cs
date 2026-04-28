using PromotionMod.Common;
using PromotionMod.Stats.Sniper;
namespace PromotionMod.Elements.PromotionAbilities.Sniper;

public class ActTargetHand : PromotionCombatAbility
{
    public override int PromotionId => Constants.FeatSniper;
    public override string PromotionString => Constants.SniperId;
    public override int AbilityId => Constants.ActTargetHandId;
    public override PromotionAbilityCostType PromotionAbilityCost => PromotionAbilityCostType.PromotionAbilityCostMana;
    public override bool CanPerformExtra(bool verbose)
    {
        if (CC.GetBestRangedWeapon() == null)
        {
            if (CC.IsPC && verbose) Msg.Say("sniper_needrangedweapon".langGame());
            return false;
        }

        return ACT.Ranged.CanPerform();
    }

    public override bool Perform()
    {
        ConSniperTarget sniperTarget = CC.AddCondition<ConSniperTarget>(GetPower(CC)) as ConSniperTarget;
        sniperTarget.Target = ConSniperTarget.TargetPart.Hand;
        // Perform a Ranged attack at the target.
        Thing rangedWeapon = CC.GetBestRangedWeapon();
        CC.ranged = rangedWeapon;
        ACT.Ranged.Perform(CC, TC);
        sniperTarget.Kill();
        return true;
    }
}
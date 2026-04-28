using PromotionMod.Common;
using PromotionMod.Stats.Sentinel;
namespace PromotionMod.Elements.PromotionAbilities.Sentinel;

public class ActShieldSmite : PromotionCombatAbility
{
    public override int PromotionId => Constants.FeatSentinel;
    public override string PromotionString => Constants.SentinelId;
    public override int AbilityId => Constants.ActShieldSmiteId;

    public override PromotionAbilityCostType PromotionAbilityCost => PromotionAbilityCostType.PromotionAbilityCostMana;

    public override bool CanPerformExtra(bool verbose)
    {
        if (CC.body.GetAttackStyle() is not AttackStyle.Shield)
        {
            if (CC.IsPC && verbose) Msg.Say("sentinel_needshield".langGame());
            return false;
        }
        if (TC is not { isChara: true }) return false;
        return ACT.Melee.CanPerform();
    }

    public override bool Perform()
    {
        CC.AddCondition<ConShieldSmiteAttack>(GetPower(CC), true);
        CC.PlaySound("shield_bash");
        return new ActMelee().Perform(CC, TC);
    }
}
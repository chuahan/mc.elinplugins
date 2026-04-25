using PromotionMod.Common;
using PromotionMod.Stats.Berserker;
namespace PromotionMod.Elements.PromotionAbilities.Berserker;

public class ActLifebreak : PromotionCombatAbility
{
    public override int PromotionId => Constants.FeatBerserker;
    public override string PromotionString => Constants.BerserkerId;
    public override int AbilityId => Constants.ActLifebreakId;

    public override bool CanPerformExtra()
    {
        if (TC is not { isChara: true }) return false;
        return ACT.Melee.CanPerform();
    }

    public override bool Perform()
    {
        CC.AddCondition<ConLifebreakAttack>(GetPower(CC), true);
        new ActMelee().Perform(CC, TC);
        return true;
    }
}
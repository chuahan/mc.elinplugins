using PromotionMod.Common;
using PromotionMod.Stats.Ranger;
namespace PromotionMod.Elements.PromotionAbilities.Ranger;

public class ActGimmickCoating : PromotionCombatAbility
{
    public override int PromotionId => Constants.FeatRanger;
    public override string PromotionString => Constants.RangerId;
    public override int AbilityId => Constants.ActGimmickCoatingId;

    public override PromotionAbilityCostType PromotionAbilityCost => PromotionAbilityCostType.PromotionAbilityCostMana;
    
    public override bool CanPerformExtra()
    {
        if (CC.HasCooldown(Constants.ActGimmickCoatingId)) return false;
        return base.CanPerform();
    }

    public override bool Perform()
    {
        CC.AddCondition<ConGimmickCoating>(GetPower(CC));
        CC.AddCooldown(Constants.ActGimmickCoatingId, 5);
        return true;
    }
}
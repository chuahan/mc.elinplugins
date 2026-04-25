using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Ranger;

public class StRangersCanto : PromotionAbility
{
    public override int PromotionId => Constants.FeatRanger;
    public override string PromotionString => Constants.RangerId;
    public override int AbilityId => Constants.StRangersCantoId;
    public override PromotionAbilityCostType PromotionAbilityCost => PromotionAbilityCostType.PromotionAbilityCostNone;

    public override bool CanPerformExtra()
    {
        // CC must be riding or be a parasite.
        return CC.ride != null;
    }
}
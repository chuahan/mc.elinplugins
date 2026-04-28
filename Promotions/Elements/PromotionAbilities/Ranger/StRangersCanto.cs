using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Ranger;

public class StRangersCanto : PromotionAbility
{
    public override int PromotionId => Constants.FeatRanger;
    public override string PromotionString => Constants.RangerId;
    public override int AbilityId => Constants.StRangersCantoId;
    public override PromotionAbilityCostType PromotionAbilityCost => PromotionAbilityCostType.PromotionAbilityCostNone;

    public override bool CanPerformExtra(bool verbose)
    {
        // CC must be riding or be a parasite.
        if (CC.ride == null)
        {
            if (CC.IsPC && verbose) Msg.Say("ranger_rangerscanto_mustberiding".langGame());
            return false;
        }

        return true;
    }
}
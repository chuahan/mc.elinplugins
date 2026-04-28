using System.Linq;
using PromotionMod.Common;
using PromotionMod.Stats.Machinist;
namespace PromotionMod.Elements.PromotionAbilities.Machinist;

public class StHeavyarms : PromotionAbility
{
    public override int PromotionId => Constants.FeatMachinist;
    public override string PromotionString => Constants.MachinistId;

    public override int AbilityId => Constants.StHeavyarmsId;

    public override bool CanPerformExtra(bool verbose)
    {
        // Can cancel out at any time.
        if (CC.HasCondition<StanceHeavyarms>())
        {
            return true;
        }

        // Cannot be used while riding or as a parasite.
        if (CC.ride != null)
        {
            if (CC.IsPC && verbose) Msg.Say("machinist_heavyarms_noride".langGame());
            return false;
        }

        // Can only be used if there are visible targets.
        if (!CC.fov.ListPoints().Any(p => p.Charas.Any(c => c.IsHostile(CC))))
        {
            if (CC.IsPC && verbose) Msg.Say("machinist_heavyarms_notargets".langGame());
            return false;
        }

        return true;
    }
}
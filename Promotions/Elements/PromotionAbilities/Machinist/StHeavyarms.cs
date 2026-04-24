using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Stats.Machinist;
namespace PromotionMod.Elements.PromotionAbilities.Machinist;

public class StHeavyarms : PromotionAbility
{
    public override int PromotionId => Constants.FeatMachinist;
    public override string PromotionString => Constants.MachinistId;

    public override int AbilityId => Constants.StHeavyarmsId;

    public override bool CanPerformExtra()
    {
        // Can cancel out at any time.
        if (CC.HasCondition<StanceHeavyarms>())
        {
            return true;
        }

        // Cannot be used while riding or as a parasite.
        if (CC.ride != null)
        {
            if (CC.IsPC) Msg.Say("machinist_heavyarms_noride".langGame());
            return false;
        }

        // Can only be used if there are visible targets.
        List<Point> targets = new List<Point>();
        foreach (Point p in CC.fov.ListPoints())
        {
            foreach (Chara c in p.Charas)
            {
                if (!c.IsHostile(CC)) continue;
                targets.Add(p);
                break;
            }
        }

        if (targets.Count == 0)
        {
            if (CC.IsPC) Msg.Say("machinist_heavyarms_notargets".langGame());
            return false;
        }

        return true;
    }
}
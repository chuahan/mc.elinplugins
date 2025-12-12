using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Machinist;

public class StHeavyarms : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatMachinist) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.MachinistId.lang()));
            return false;
        }
        
        // Cannot be used while riding or as a parasite.
        if (CC.ride != null) return false;

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
        
        return targets.Count != 0 && base.CanPerform();

    }

    // This ability doesn't cost MP or Stamina to activate, and instead will drain 5% mana per turn when active. 
    public override Cost GetCost(Chara c)
    {
        return new Cost
        {
            cost = 0,
            type = CostType.None
        };
    }
}
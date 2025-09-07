using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Sniper;

public class ActSpreadShot : Ability
{
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatSniper) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.SniperId.lang()));
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
        List<Point> coneRange = _map.ListPointsInArc(CC.pos, TP, 10, 35f);
        List<Chara> targets = new List<Chara>();
        foreach (Point p in coneRange)
        {
            if (p.Charas != null)
            {
                foreach (Chara chara in p.Charas.Where(chara => chara.IsHostile(CC) && !targets.Contains(CC)))
                {
                    targets.Add(chara);
                }
            }
        }

        foreach (Chara target in targets)
        {
            // Perform a Ranged attack at the target.
            TweenUtil.Delay(0.7F, delegate
            {
                ACT.Ranged.Perform(CC, target);
            });
        }

        return true;
    }
}
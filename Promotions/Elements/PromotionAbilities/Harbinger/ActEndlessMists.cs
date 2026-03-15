using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Stats.Harbinger;
namespace PromotionMod.Elements.PromotionAbilities.Harbinger;

/// <summary>
///     Harbinger Ability
///     Extends the duration of all the Miasma debuffs on nearby enemies.
/// </summary>
public class ActEndlessMists : Ability
{

    private float _effectRadius = 5F;
    public override bool ShowMapHighlight => true;

    public override bool CanPerform()
    {
        if (!CC.MatchesPromotion(Constants.FeatHarbinger))
        {
            Msg.Say("classlocked_ability".lang(Constants.HarbingerId.lang()));
            return false;
        }
        if (CC.HasCooldown(Constants.ActEndlessMistsId)) return false;
        bool basePerform = base.CanPerform();
        
        return basePerform;
    }

    public override void OnMarkMapHighlights()
    {
        if (!scene.mouseTarget.pos.IsValid)
        {
            return;
        }
        List<Point> list = _map.ListPointsInCircle(scene.mouseTarget.pos, _effectRadius);
        if (list.Count == 0)
        {
            list.Add(CC.pos.Copy());
        }
        foreach (Point item in list)
        {
            item.SetHighlight(8);
        }
    }

    public override bool Perform()
    {
        foreach (Chara target in HelperFunctions.GetCharasWithinRadius(CC.pos, _effectRadius, CC, false, true))
        {
            List<Condition> conditions = target.conditions.Where(x => x is ConHarbingerMiasma or ConMiasma).ToList();
            foreach (Condition condition in conditions)
            {
                condition.Mod(1);
            }
        }

        CC.AddCooldown(Constants.ActEndlessMistsId, 5);
        return true;
    }
}
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
    public override bool ShowMapHighlight => true;
    
    private float _effectRadius = 5F;
    
    public override bool CanPerform()
    {
        if (CC.Evalue(Constants.FeatHarbinger) == 0)
        {
            Msg.Say("classlocked_ability".lang(Constants.HarbingerId.lang()));
            return false;
        }
        if (CC.HasCooldown(Constants.ActEndlessMistsId)) return false;
        bool basePerform = base.CanPerform();

        // This... doesn't work ._.
        /*
        if (basePerform && CC.IsPC)
        {
            List<Point> list = EClass._map.ListPointsInCircle(CC.pos, _effectRadius, true, true);
            if (list.Count == 0)
            {
                list.Add(Act.CC.pos.Copy());
            }
            foreach (Point item in list)
            {
                item.SetHighlight(8);   
            }
        }
        */
        return basePerform;
    }
    
    public override void OnMarkMapHighlights()
    {
        if (!EClass.scene.mouseTarget.pos.IsValid)
        {
            return;
        }
        List<Point> list = EClass._map.ListPointsInCircle(EClass.scene.mouseTarget.pos, _effectRadius, true, true);
        if (list.Count == 0)
        {
            list.Add(Act.CC.pos.Copy());
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
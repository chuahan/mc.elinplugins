using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Gambler;

/// <summary>
/// </summary>
public class ActSpinSlots : PromotionSpellAbility
{


    private float _effectRadius = 5F;
    public override int PromotionId => Constants.FeatGambler;
    public override string PromotionString => Constants.GamblerId;
    public override int AbilityId => Constants.ActSpinSlotsId;
    public override bool ShowMapHighlight => true;

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
        // TODO: Implement.
        return true;
    }
}
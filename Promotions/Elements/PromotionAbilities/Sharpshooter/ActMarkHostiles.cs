using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Stats.Sharpshooter;
namespace PromotionMod.Elements.PromotionAbilities.Sharpshooter;

public class ActMarkHostiles : Ability
{
    private float _effectRadius = 3F;

    public override bool CanPerform()
    {
        if (!CC.MatchesPromotion(Constants.FeatSharpshooter))
        {
            Msg.Say("classlocked_ability".lang(Constants.SharpshooterId.lang()));
            return false;
        }

        return base.CanPerform();
    }

    public override Cost GetCost(Chara c)
    {
        Cost convertToMp = base.GetCost(c);
        convertToMp.type = CostType.MP;
        return convertToMp;
    }

    public override void OnMarkMapHighlights()
    {
        List<Point> list = _map.ListPointsInCircle(CC.pos, _effectRadius);
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
        int manaRestore = (int)(CC.mana.max * 0.05F);
        foreach (Chara target in HelperFunctions.GetCharasWithinRadius(TP, _effectRadius, CC, false, true))
        {
            target.AddCondition<ConMarked>();
            CC.mana.Mod(manaRestore);
        }
        return true;
    }
}
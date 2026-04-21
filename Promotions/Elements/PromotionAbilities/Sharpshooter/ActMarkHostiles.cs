using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Stats.Sharpshooter;
namespace PromotionMod.Elements.PromotionAbilities.Sharpshooter;

public class ActMarkHostiles : PromotionCombatAbility
{

    private float _effectRadius = 3F;
    public override int PromotionId => Constants.FeatSharpshooter;
    public override string PromotionString => Constants.SharpshooterId;
    public override int AbilityId => Constants.ActMarkHostilesId;

    public override PromotionAbilityCostType PromotionAbilityCost => PromotionAbilityCostType.PromotionAbilityCostMana;


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
using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/*
 * The story of ancient valor. The Knightcaller performs in otherworldly songs that calls forth aid from a ghostly knight order.
 * Knightcallers focus on summoning a force of knights from the past through song to go to war once again.
 * They specialize in summoning special minions and have minion specific support spells.
 */
public class FeatKnightcaller : PromotionFeat
{
    public override string PromotionClassId => Constants.KnightcallerId;
    public override int PromotionClassFeatId => Constants.FeatKnightcaller;
    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActSpiritRageId,
        Constants.ActSpiritRallyId
    };
    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "pianist";
    }

    protected override void ApplyInternal()
    {
        // Farming - 286
        owner.Chara.elements.ModPotential(286, 30);
        // Casting - 304
        owner.Chara.elements.ModPotential(304, 30);
    }
}
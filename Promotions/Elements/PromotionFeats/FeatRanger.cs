using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/*
 * The guardian of the woodlands. Rangers are survivalists who favor the forests and the wildlife opposed to civilization.
 * Rangers focus on mobility and survivability, utilizing traps and light feet to keep themselves out of harms way.
 * They specialize in debuffing enemies and laying down covering fire from the backlines.
 */
public class FeatRanger : PromotionFeat
{
    public override string PromotionClassId => Constants.RangerId;
    public override int PromotionClassFeatId => Constants.FeatRanger;
    public override List<int> PromotionAbilities => new List<int>
    {
    };
    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "archer";
    }

    protected override void ApplyInternal()
    {
        // Farming - 286
        owner.Chara.elements.ModPotential(286, 30);
        // Casting - 304
        owner.Chara.elements.ModPotential(304, 30);
    }
}
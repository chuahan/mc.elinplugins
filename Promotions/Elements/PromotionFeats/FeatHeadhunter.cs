using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/*
 * The head is where the man is. The Headhunter is infallible, the axe will drop sooner or later.
 * Headhunters focus on opportunistic killing blows to strengthen themselves.
 * They specialize in last hitting enemies to grant themselves stacking buffs, slowly becoming an unstoppable force as the battle goes on.
 */
public class FeatHeadhunter : PromotionFeat
{
    public override string PromotionClassId => Constants.HeadhunterId;
    public override int PromotionClassFeatId => Constants.FeatHeadhunter;
    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActCullingBlowId
    };
    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "executioner";
    }

    protected override void ApplyInternal()
    {
        // Farming - 286
        owner.Chara.elements.ModPotential(286, 30);
        // Casting - 304
        owner.Chara.elements.ModPotential(304, 30);
    }
}
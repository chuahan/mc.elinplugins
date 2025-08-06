using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/*
 * A bolt from the blue. Snipers are distinguished archers who have honed their skills to the utmost.
 * Snipers focus on aiming at specific points on a target to achieve different effects.
 * They specialize in removing priority targets from the fight, either permanently through a vital point, or temporarily through crippling them.
 */
public class FeatSniper : PromotionFeat
{
    public override string PromotionClassId => Constants.SniperId;
    public override int PromotionClassFeatId => Constants.FeatSniper;
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
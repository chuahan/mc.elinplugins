using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/*
 * The iron fist of judgement. To see a Justicar is to see fear, and to pity the one who broke it.
 * Justicars focus on shifting momentum through intimidation and execution.
 * They specialize in singling out targets and bringing them down, and breaking enemy morale in the process.
 */
public class FeatJusticar : PromotionFeat
{
    public override string PromotionClassId => Constants.JusticarId;
    public override int PromotionClassFeatId => Constants.FeatJusticar;
    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActIntimidateId
    };
    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "inquisitor";
    }

    protected override void ApplyInternal()
    {
        // Farming - 286
        owner.Chara.elements.ModPotential(286, 30);
        // Casting - 304
        owner.Chara.elements.ModPotential(304, 30);
    }
}
using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/*
 * A champion of technological revolution. The Machinist uses technology and firearms to face the oncoming threats with overwhelming firepower.
 * Machinists focus on the creative use of firearms with modifications and automation.
 * They specialize in active modifications to their weapons to improve their performance for the situation at hand.
 */
public class FeatMachinist : PromotionFeat
{
    public override string PromotionClassId => Constants.MachinistId;
    public override int PromotionClassFeatId => Constants.FeatMachinist;
    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActLoadUpId,
        Constants.ActOverclockId
    };
    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "gunner";
    }

    protected override void ApplyInternal()
    {
        // Farming - 286
        owner.Chara.elements.ModPotential(286, 30);
        // Casting - 304
        owner.Chara.elements.ModPotential(304, 30);
    }
}
using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/*
 * The shooting star that pierces the veil of night. The Luminary is guiding light that pierces the enemy.
 * Luminaries focus on paving the way, holding their own in combat in addition to protecting their allies.
 * Luminaries specialize in closing in at the start of battle and doing enough damage to shift the momentum to their side.
 */
public class FeatLuminary : PromotionFeat
{
    public override string PromotionClassId => Constants.LuminaryId;
    public override int PromotionClassFeatId => Constants.FeatLuminary;
    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.VanguardStanceId,
        Constants.ActDeflectId
    };
    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "paladin";
    }

    protected override void ApplyInternal()
    {
        // Farming - 286
        owner.Chara.elements.ModPotential(286, 30);
        // Casting - 304
        owner.Chara.elements.ModPotential(304, 30);
    }
}
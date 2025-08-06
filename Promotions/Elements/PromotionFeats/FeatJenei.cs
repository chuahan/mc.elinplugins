using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/*
 * Shapers of the natural energies. Jenei are those who call upon the natural forces of Earth, Fire, Air, and Water.
 * Jenei focus on combining the four natural elements to summon great representatives of the natural forces to aid them in combat.
 * They specialize in invoking mighty spells with a variety of effects through building up amounts of each of the elements.
 */
public class FeatJenei : PromotionFeat
{
    public override string PromotionClassId => Constants.JeneiId;
    public override int PromotionClassFeatId => Constants.FeatJenei;
    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActSpiritSummonId
    };
    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "farmer";
    }

    protected override void ApplyInternal()
    {
        // Farming - 286
        owner.Chara.elements.ModPotential(286, 30);
        // Casting - 304
        owner.Chara.elements.ModPotential(304, 30);
    }
}
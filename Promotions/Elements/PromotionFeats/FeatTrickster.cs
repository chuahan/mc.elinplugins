using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/*
 * The nightmare harlequin. A master in the illusion arts, Tricksters specialize in deception and debilitation.
 * Tricksters focus on sowing discord and chaos through the enemy ranks through traps and illusions.
 * They specialize in applying and then exploiting debuffs to deal damage or buff their own forces.
 */
public class FeatTrickster : PromotionFeat
{
    public override string PromotionClassId => Constants.TricksterId;
    public override int PromotionClassFeatId => Constants.FeatTrickster;
    public override List<int> PromotionAbilities => new List<int>
    {
    };
    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "thief";
    }

    protected override void ApplyInternal()
    {
        // Farming - 286
        owner.Chara.elements.ModPotential(286, 30);
        // Casting - 304
        owner.Chara.elements.ModPotential(304, 30);
    }
}
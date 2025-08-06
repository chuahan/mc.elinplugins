using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/*
 * The protection of the Hawk's Eye. The Sharpshooter provides supporting fire for your forces with their preferred long distance firearms.
 * Sharpshooters focus on choosing targets, and eliminating the targets, providing a killzone to cover your team.
 * They specialize in staying stationary while laying down deadly accurate gunfire to eliminate foes.
 */
public class FeatSharpshooter : PromotionFeat
{
    public override string PromotionClassId => Constants.SharpshooterId;
    public override int PromotionClassFeatId => Constants.FeatSharpshooter;
    public override List<int> PromotionAbilities => new List<int>
    {
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
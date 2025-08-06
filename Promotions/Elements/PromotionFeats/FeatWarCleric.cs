using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/*
 * The one to show the way, whether you like it or not. The War Cleric has taken up violence as an alternative to compassion.
 * War Clerics focus on being durable enough to move to the front lines to rescue beleaguered allies or bring down the hammer on enemies.
 * They specialize in being able to provide curative support or frontline strength as needed, no matter the situation.
 */
public class FeatWarCleric : PromotionFeat
{
    public override string PromotionClassId => Constants.WarClericId;
    public override int PromotionClassFeatId => Constants.FeatWarCleric;
    public override List<int> PromotionAbilities => new List<int>
    {
    };
    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "priest";
    }

    protected override void ApplyInternal()
    {
        // Farming - 286
        owner.Chara.elements.ModPotential(286, 30);
        // Casting - 304
        owner.Chara.elements.ModPotential(304, 30);
    }
}
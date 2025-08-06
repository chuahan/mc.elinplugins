using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/*
 * A king is no king without his people, but a people without their king would be lost as well. The Sovereign is a frontline leader that is as much a tactician as they are a warrior.
 * Sovereigns focus on observing the battle and commanding their forces as the situation sees fit.
 * They specialize in the use of Law and Chaos Stances and the subsequent Orders to change the flow of battle.
 */
public class FeatSovereign : PromotionFeat
{
    public override string PromotionClassId => Constants.SovereignId;
    public override int PromotionClassFeatId => Constants.FeatSovereign;
    public override List<int> PromotionAbilities => new List<int>
    {
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
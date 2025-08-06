using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/*
 * Suffer not the heretic. Witch Hunters have decided they really hate mages, and will go through any means to see them gone.
 * Witch Hunters focus on hunting down casters with their anti-magic in combat.
 * They specialize in being the absolute nightmare of any mage on the field through massive spell resistances and anti-casting abilities.
 */
public class FeatWitchHunter : PromotionFeat
{
    public override string PromotionClassId => Constants.WitchHunterId;
    public override int PromotionClassFeatId => Constants.FeatWitchHunter;

    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActManaBreakId
    };

    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "inquisitor";
    }

    protected override void ApplyInternal()
    {
        // Gun Skill - 105
        owner.Chara.elements.ModPotential(105, 30);
        // Crossbow Skill - 109
        owner.Chara.elements.ModPotential(109, 30);
        // Sword - 101
        owner.Chara.elements.ModPotential(101, 30);
        // Will - 75
        owner.Chara.elements.ModPotential(175, 10);
        // Base Antimagic - 93
        owner.Chara.elements.ModBase(93, 30);
    }
}
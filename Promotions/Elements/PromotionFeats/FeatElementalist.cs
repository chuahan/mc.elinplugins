using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
/// The harbinger of the storm. Sometimes called Arsenal Magi, Elementalists are wizards who wield the entire spectrum of elements.
/// Elementalists focus on casting a variety of spells across all elements.
/// They specialize in continuous casting of spells, stockpiling the elemental orbs and able to combine them all into a single burst as needed.
/// Skill - Torrent Conspectus - Summons more bits at one time, and they spawn boosted.
/// Skill - Elemental Fury - Summons a massive damaging elemental storm that harms nearby enemies.
/// Skill - Flare - Does massive void-typed damage to a single target.
/// </summary>
public class FeatElementalist : PromotionFeat
{
    public override string PromotionClassId => Constants.ElementalistId;
    public override int PromotionClassFeatId => Constants.FeatElementalist;
    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActElementalFuryId,
        Constants.ActFlareId
    };
    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "wizard";
    }

    protected override void ApplyInternal()
    {
        // Farming - 286
        owner.Chara.elements.ModPotential(286, 30);
        // Casting - 304
        owner.Chara.elements.ModPotential(304, 30);
    }
}
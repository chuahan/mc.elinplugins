using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
/// The cloak and dagger. Hermits are stealthy warriors that are unnoticed in battles, only seen briefly by foes shortly prior to their demise.
/// Hermits focus on choosing and eliminating high priority targets from the shadows.
/// They specialize in hyperfocusing on a single target, gaining advantages specifically against them.
/// Skill - Mark For Death - Marks an enemy for death, increasing your performance against that target.
/// Skill - Shadow Shroud - Hide in the Shadows, making yourself harder to detect. This condition has a chance to be lifted when you perform any attack.
/// Skill - Assassinate - When attacking an enemy Marked for Death, inflicts a grievous wound.
/// Passive - Opportunist - Increased performance against enemies inflicted with Bad statuses.
/// </summary>
public class FeatHermit : PromotionFeat
{
    public override string PromotionClassId => Constants.HermitId;
    public override int PromotionClassFeatId => Constants.FeatHermit;
    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActMarkForDeathId,
        Constants.ActShadowShroudId,
        Constants.ActAssassinateId
    };
    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "thief";
    }

    protected override void ApplyInternal()
    {
        // Stealth
        // Short Swords
        // Evasion
        //owner.Chara.elements.ModPotential(286, 30);
        owner.Chara.elements.ModPotential(304, 30);
    }
}
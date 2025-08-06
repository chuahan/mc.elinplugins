using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
/// The cloak and dagger. Hermits are stealthy warriors that are unnoticed in battles, only seen briefly by foes shortly prior to their demise.
/// Hermits focus on choosing and eliminating high priority targets from the shadows.
/// They specialize in hyperfocusing on a single target, gaining advantages specifically against them. 
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
        // Throwing

        //owner.Chara.elements.ModPotential(286, 30);
        owner.Chara.elements.ModPotential(304, 30);
    }
}
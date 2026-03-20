using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
///     The cloak and dagger. Hermits are stealthy warriors that are unnoticed in battles, only seen briefly by foes
///     shortly prior to their demise.
///     Hermits focus on choosing and eliminating high priority targets from the shadows.
///     They specialize in hyperfocusing on a single target, gaining advantages specifically against them.
///     Skill - Mark For Death - Marks an enemy for death, increasing your performance against that target.
///     Skill - Shadow Shroud - Hide in the Shadows, making yourself harder to detect. This condition has a chance to be
///     lifted when you perform any attack.
///     Skill - Assassinate - When attacking an enemy Marked for Death, inflicts a grievous wound.
///     Skill - Preparation. Targetted debuff that also grants the Hermit Crit Boost.
///         Also inflicts one of: Sleep, Poison, Paralyze, Bleed, or Faint.
///         The chance of inflicting the debuff increases depending on how high the Stalk value of Mark for Death is.
///         This ability can only be used on targets that have been Marked for Death. 
///     Passive - Opportunist - Increased performance against enemies inflicted with Bad statuses.
/// </summary>
public class FeatHermit : PromotionFeat
{
    public override string PromotionClassId => Constants.HermitId;
    public override int PromotionClassFeatId => Constants.FeatHermit;

    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActMarkForDeathId,
        Constants.ActShadowShroudId,
        Constants.ActPreparationId,
        Constants.ActAssassinateId
    };

    public override string JobRequirement => "thief";

    protected override void ApplyInternalNPC(Chara c)
    {
        c.ability.Add(Constants.ActMarkForDeathId, 75, false);
        c.ability.Add(Constants.ActShadowShroudId, 100, false);
        c.ability.Add(Constants.ActPreparationId, 75, false);
        c.ability.Add(Constants.ActAssassinateId, 50, false);
    }

    override internal void _OnApply(int add, ElementContainer eleOwner, bool hint)
    {
        base._OnApply(add, eleOwner, hint);
    }
}
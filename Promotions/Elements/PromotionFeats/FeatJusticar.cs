using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
///     The iron fist of judgement. To see a Justicar is to see fear, and to pity the one who broke it.
///     Justicars focus on shifting momentum through intimidation and execution.
///     They specialize in singling out targets and bringing them down, and breaking enemy morale in the process.
///     Ability - Intimidate - Inflicts Armor break on the target. Also inflicts excommunicate. Inflicts fear on other
///     enemies near the target.
///     Ability - Subdue - Inflicts Suppress, and Attack Break on the target. Also inflicts excommunicate.
///     Ability - Condemn - Inflicts Entangle on nearby enemies. For every enemy impacted, Justicar grants their team
///     Overshield.
/// 
///     Ability - Inspire - An ability buffs nearby allies with hero
///     TODO (P2) Add a passive maybe? Or another ability.
/// </summary>
public class FeatJusticar : PromotionFeat
{
    public override string PromotionClassId => Constants.JusticarId;
    public override int PromotionClassFeatId => Constants.FeatJusticar;

    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActIntimidateId,
        Constants.ActSubdueId,
        Constants.ActCondemnId
    };

    protected override void ApplyInternalNPC(Chara c)
    {
        c.ability.Add(Constants.ActIntimidateId, 60, false);
        c.ability.Add(Constants.ActSubdueId, 60, false);
        c.ability.Add(Constants.ActCondemnId, 60, false);
    }

    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "inquisitor";
    }

    protected override void ApplyInternal()
    {
        // Farming - 286
        owner.Chara.elements.ModPotential(286, 30);
        // Casting - 304
        owner.Chara.elements.ModPotential(304, 30);
    }
}
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
///     Ability - Condemn - Inflicts Entangle on nearby enemies. For every enemy impacted, Justicar grants their team Overshield.
///
///     TODO: Implement
///     Passive - Unrelenting Advance - Justicars will gain a random chance activated effect based on their Karma (PC Faction will use the PC's Karma)
///         When using any of the Justicar abilities, you have a small chance of activating either effect which will add either the Sun or the Moon Condition to the Justicar
///         and all nearby allies.
///         This ability has a cooldown of 10 turns.
///         In a bracket, at the start of it you will have a 10% chance of activating the condition, while at complete alignment (+100 or -100) you will have a 35% chance.
///             Sigmoid scaling starting at 10%, up to 35%, based on absolute value of your karma.
///         At exactly 0 Karma, you can activate either source with 25% chance
///         Non PC Faction Justicars will count as 0 Karma.
///         Positive Karma will add the Sun - When active, you will reduce the next instance of damage you take by 50%
///         Negative Karma will add the Moon - When active, your next attack or next spell will gain 50% vorpal and pierce 1 additional tier of elemental resistances.
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
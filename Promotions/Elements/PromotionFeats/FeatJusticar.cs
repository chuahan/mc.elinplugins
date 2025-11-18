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
///     Protection.
///     Flames of Judgement - Justicars will reserve a part of their mana to activate a flaming aura around themselves.
///     Justicar gains Regeneration that scales with the spellpower.
///     Every turn, the Justicar will take 30% of their maximum health as fire damage. This same damage is also dealt to
///     nearby enemies.
///     Fire Resistance affects both outgoing and self-damage.
///     Depending on whether the Justicar has Positive or Negative Karma, there will be additional effects applied.
///     For Positive Karma: The fire will also heal other allies with each pulse for half of the damage taken by the
///     Justicar.
///     The Justicar is not healed by this effect.
///     For Negative Karma: The fire will also have a chance of inflicting Fire Break.
///     For Neutral Karma (0) This will do both effects.
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
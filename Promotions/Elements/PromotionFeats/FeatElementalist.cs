using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
///     The harbinger of the storm. Sometimes called Arsenal Magi, Elementalists are wizards who wield the entire spectrum
///     of elements.
///     Elementalists focus on casting a variety of spells across all elements.
///     They specialize in continuous casting of spells, stockpiling the elemental orbs and able to combine them all into a
///     single burst as needed.
///     Skill - Elemental Fury - Summons a massive damaging elemental storm that harms nearby enemies.
///     Skill - Flare - Does massive void-typed damage to a single target.
/// 
///     Passive - Spell Tempo - When you cast a spell of an element that you do not have stockpiled:
///         That element will inflict Elemental Break to reduce their resistances to that element.
///         You will gain a stack of Spell Tempo. Each stack of Spell Tempo will grant:
///             Speed
///             Mana Regeneration
///             Enhance Spell 
///
///     Passive - Arsenal Conspectus - When reading an elemental spellbook, will grant charges of two different elements of the same spell.
///     Passive - Torrent Bit Conversion - Summons multiple bits in a single cast. Bits start with Boost and Protection.
///     Passive - Elemental Mastery - Doubles eleP.
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

    protected override void ApplyInternalNPC(Chara c)
    {
        c.ability.Add(Constants.ActElementalFuryId, 50, false);
        c.ability.Add(Constants.ActFlareId, 50, false);
        for (int x = 0; x <= 15; x++) // Add all the Arrow spells
        {
            c.ability.Add(50500 + x, 75, false);
        }
    }

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
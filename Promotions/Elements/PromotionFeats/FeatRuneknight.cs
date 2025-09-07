using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
/// The sword of mystical judgement. The Runeknights etch magic itself into their very being, allowing them to harness
/// surrounding magical energy.
/// Runeknights focus on both physical and magical attacks, able to turn the foes spells against them.
/// They specialize in being exceptionally resistant against enemy magic spells, absorbing them to empower themselves.
/// Skill - Runic Guard - Gains Runic Guard condition. When you next take an elemental attack, you will absorb it,
/// gaining Elemental Attunement.
/// Condition - Elemental Attunement - When attuned to an element, you gain full immunity to that element, absorbing
/// the damage as MP instead.
/// When you take damage that matches your elemental attunement, that damage is absorbed into your condition instead.
/// Damage build decays by 5% per turn.
/// Skill - Spinning Slash - Releases your attunement from your condition. the stored elemental damage is added and
/// done as damage to all enemies nearby in 3 Radius via magic sword damage.
/// Skill - Rune Etching - A Rune Knight is able to create a protective talisman that can be applied to all allies.
/// These will trigger on taking an attack or damage.
/// Requires the Rune Knight to be carrying a Calligraphy Set.
/// Ally Gains ConWardingRune
/// - Every time the owner takes damage, it will lose a charge and reduce the damage taken, knock the aggressor back and attempt to apply ConParalyze
/// - Every time a debuff is aimed at the owner, it will lose a charge and negate the debuff.
/// </summary>
public class FeatRuneknight : PromotionFeat
{
    public override string PromotionClassId => Constants.RuneknightId;
    public override int PromotionClassFeatId => Constants.FeatRuneknight;

    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActRunicGuardId,
        Constants.ActSpinningSlashId,
        Constants.ActRuneEtchingId
    };

    protected override void ApplyInternalNPC(Chara c)
    {
        c.ability.Add(Constants.ActRunicGuardId, 75, false);
        c.ability.Add(Constants.ActSpinningSlashId, 75, false);
        c.ability.Add(Constants.ActRuneEtchingId, 100, true);
    }

    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "swordsage";
    }

    protected override void ApplyInternal()
    {
        // Longsword -
        // Casting - 304
        // Shield
        owner.Chara.elements.ModPotential(286, 30);
        owner.Chara.elements.ModPotential(304, 30);
    }
}
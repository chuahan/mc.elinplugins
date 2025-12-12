using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
///     The reason to fear the darkness. The Harbinger advances while spreading gloom with each step.
///     Harbingers focus on spreading a myriad of miasma to their advantage.
///     Harbingers specialize in covering the battlefield in various kinds of miasma, cutting down stricken enemies with
///     their attacks.
///     Skill - Endless Mists - Extends debuff duration on enemies of Harbinger Miasmas
///     Skill - Accursed Touch - Applies a Harbinger Miasma to the target
///     Skill - Gloom - Stance that reserves mana and emits a debuffing aura at a radius of 3. Inflicts a random Harbinger Miasma.
///
///     Passive - Miasma Hunter - Gains increased damage if the target has miasma on them.
///     Passive - Miasma Armor - Every time Miasma is applied to an enemy, you gain a stack of damage reduction.
///     Passive - Miasma Mastery - Absorbs nearby Miasmas from allies.
///     Passive - Conspectus of Miasma - Can convert Elemental ATtack spells into Miasmas of the same kind. 
/// </summary>
public class FeatHarbinger : PromotionFeat
{
    public override string PromotionClassId => Constants.HarbingerId;
    public override int PromotionClassFeatId => Constants.FeatHarbinger;

    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActEndlessMistsId,
        Constants.ActAccursedTouchId,
        Constants.StGloomId,
    };

    protected override void ApplyInternalNPC(Chara c)
    {
        c.ability.Add(Constants.ActEndlessMistsId, 75, false);
        c.ability.Add(Constants.ActAccursedTouchId, 90, false);
    }

    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "executioner";
    }
    
    override internal void _OnApply(int add, ElementContainer eleOwner, bool hint)
    {
        base._OnApply(add,eleOwner, hint);
    }
}
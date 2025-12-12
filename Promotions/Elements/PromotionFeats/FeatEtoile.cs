using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
///     The shooting star that pierces the veil of night. The Etoile is guiding light that pierces the enemy.
///     Luminaries focus on paving the way, holding their own in combat in addition to protecting their allies.
///     Luminaries specialize in closing in at the start of battle and doing enough damage to shift the momentum to their
///     side.
///     Skill - Vanguard Stance - A stance that redirects all damage done to nearby non-summon allies to you. Basically a
///     stance wall of flesh.
///     Skill - Light Wave - Charges through all enemies to a specific point. Knocks them away. For every enemy in the
///     path, does damage and summons a Holy Sword Bit.
///     Skill - Parry - Enter a Parrying stance for one turn. If you take damage while you are Parrying:
///     Reduce damage by 100%.
///     Reduces the cooldown of Parry to 0 (Recharges it instantly)
///     Refunds the Mana cost.
///     Summons a Holy Sword Bit.
///     Passive - Wake of the Trailblazer - Every time Light wave hits an enemy, or an attack is parried, gain stacks of Class condition
///     Etoile takes reduced damage per stack.
///
///     TODO: Rename to Holy Knight
///         Vanguard Remains the Same
///         Light Wave -> Spearhead
///         Rename Parry -> Deflection
///         Add Sol -> Add condition to heal 30% damage as life.
///         Add Aegis as a passive - Reduce incoming damage by 50% with a chance.
/// </summary>
public class FeatEtoile : PromotionFeat
{
    public override string PromotionClassId => Constants.EtoileId;
    public override int PromotionClassFeatId => Constants.FeatEtoile;

    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.StVanguardId,
        Constants.ActLightWaveId,
        Constants.ActLuminousDeflectionId
    };

    protected override void ApplyInternalNPC(Chara c)
    {
        c.ability.Add(Constants.StVanguardId, 100, false);
        c.ability.Add(Constants.ActLightWaveId, 75, false);
        c.ability.Add(Constants.ActLuminousDeflectionId, 75, false);
    }

    protected override bool Requirement()
    {
        return (EClass.pc != null && EClass.pc.GetFlagValue(Constants.EtoilePromotionUnlockedFlag > 0));
    }
    
    override internal void _OnApply(int add, ElementContainer eleOwner, bool hint)
    {
        base._OnApply(add,eleOwner, hint);
    }
}
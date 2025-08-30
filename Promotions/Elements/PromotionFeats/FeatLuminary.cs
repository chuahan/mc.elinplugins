using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
/// The shooting star that pierces the veil of night. The Luminary is guiding light that pierces the enemy.
/// Luminaries focus on paving the way, holding their own in combat in addition to protecting their allies.
/// Luminaries specialize in closing in at the start of battle and doing enough damage to shift the momentum to their side.
/// Skill - Vanguard Stance - A stance that redirects all damage done to nearby non-summon allies to you. Basically a stance wall of flesh.
/// Skill - Light Wave - Charges through all enemies to a specific point. Knocks them away. For every enemy in the path, does damage and summons a Holy Sword Bit.
/// Skill - Parry - Enter a Parrying stance for one turn. If you take damage while you are Parrying:
///     Reduce damage by 100%.
///     Reduces the cooldown of Parry to 0 (Recharges it instantly)
///     Refunds the Mana cost.
///     Summons a Holy Sword Bit.
/// Passive - Wake of the Trailblazer - Every time Vanguard redirects damage, Light wave hits an enemy, or an attack is parried, gain stacks of Class condition
///     Luminary takes reduced damage per stack.
/// </summary>
public class FeatLuminary : PromotionFeat
{
    public override string PromotionClassId => Constants.LuminaryId;
    public override int PromotionClassFeatId => Constants.FeatLuminary;
    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.VanguardStanceId,
        Constants.ActLightWaveId,
        Constants.ActParryId,
    };
    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "paladin";
    }

    protected override void ApplyInternal()
    {
        // Longsword - 286
        //owner.Chara.elements.ModPotential(286, 30);
        // Casting - 304
        // Dual Wielding 
    }
}
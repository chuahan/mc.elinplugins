using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
///     A bolt from the blue. Snipers are distinguished archers who have honed their skills to the utmost.
///     Snipers focus on aiming at specific points on a target to achieve different effects.
///     They specialize in removing priority targets from the fight, either permanently through a vital point, or
///     temporarily through crippling them.
///     Skill - Target Head - Makes a ranged attack that on hit will Silence and Dim the target. This shot has a 25% chance
///     to cull targets at 30% or less HP.
///     Skill - Target Hand - Makes a ranged attack that on hit will disable melee and ranged attacks.
///     Skill - Target Legs - Makes a ranged attack that on hit will slow the target.
///     Skill - Spread Shot - Makes ranged attacks against all targets in a cone.
///     Passive Condition - Repetition - When making successive ranged (main) attacks against a target, snipers inflict a debuff.
///         - On the 3rd shot, Snipers will inflict Armor Break.
///         - On the 4th shot, Snipers will inflict Attack Break.
///         - 
///     Passive Condition - No Distractions - When there are no enemies within 3F radius, you will gain a condition that
///     increases Crit, Accuracy, and RapidFire.
///     Skill - Tactical Retreat - Can only be used in melee range. Leap backwards 3 tiles away and make a ranged attack upon landing.
///     Passive - Sniper's Pride - When taking a ranged attack, even if it was a miss, you will automatically retaliate with your own ranged attack.
///     
/// </summary>
public class FeatSniper : PromotionFeat
{
    public override string PromotionClassId => Constants.SniperId;
    public override int PromotionClassFeatId => Constants.FeatSniper;

    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.ActTargetHeadId,
        Constants.ActTargetHandId,
        Constants.ActTargetLegsId,
        Constants.ActSpreadShotId,
        Constants.ActTacticalRetreatId,
    };

    public override string JobRequirement => "archer";

    protected override void ApplyInternalNPC(Chara c)
    {
        c.ability.Add(Constants.ActChargedShotId, 85, false);
        c.ability.Add(Constants.ActTargetHandId, 85, false);
        c.ability.Add(Constants.ActTargetLegsId, 85, false);
        c.ability.Add(Constants.ActSpreadShotId, 70, false);
        c.ability.Add(Constants.ActTacticalRetreatId, 50, false);
    }

    override internal void _OnApply(int add, ElementContainer eleOwner, bool hint)
    {
        base._OnApply(add, eleOwner, hint);
    }
}
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
///     Passive Condition - No Distractions - When there are no enemies within 3F radius, you will gain a condition that
///     increases Crit, Accuracy, and RapidFire.
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
        Constants.ActSpreadShotId
    };

    protected override void ApplyInternalNPC(Chara c)
    {
        c.ability.Add(Constants.ActChargedShotId, 85, false);
        c.ability.Add(Constants.ActTargetHandId, 85, false);
        c.ability.Add(Constants.ActTargetLegsId, 85, false);
        c.ability.Add(Constants.ActSpreadShotId, 70, false);
    }

    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "archer";
    }
    
    override internal void _OnApply(int add, ElementContainer eleOwner, bool hint)
    {
        base._OnApply(add,eleOwner, hint);
    }
}
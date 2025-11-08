using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionFeats;

/// <summary>
///     The protection of the Hawk's Eye. The Sharpshooter provides supporting fire for your forces with their preferred
///     long distance firearms.
///     Sharpshooters focus on eliminating individual targets while providing a killzone to cover your team.
///     They specialize in staying stationary while laying down deadly accurate gunfire to eliminate foes.
///     Skill - Overwatch Stance - Stance that lowers speed, but increases hit chance, crit range, and FOV. Every tick will
///     apply Overwatched to all enemies in range.
///     The Sharpshooter will make an instant shot attack against any target that moves while affected by Overwatch.
///     Will not consume ammo when firing shots as Overwatch reaction.
///     When taking damage in Overwatch stance, exit Overwatch, gain a turn of increased Speed and stealth.
///     Skill - Charged Shot - Consumes 25% of your remaining mana. Gain Vorpal + Drill. Gain. Damage based on the mana
///     consumed. Reduces speed. Can be casted multiple times to stack the effect
///     "I won't miss."
///     "All or nothing!"
///     "Watch for friendly fire!"
///     Skill - Mark Hostiles - Applies a Marked debuff that reduces DV rating for all enemies in the area.
///     Costs Stamina.
///     Every target Marked restores 5% Mana.
///     Passive - Suppressive Fire - Ranged attacks will always apply Suppression on the target, even on miss.
///
/// TODO: Implement Passive, Implement increased FOV.
/// 
/// </summary>
public class FeatSharpshooter : PromotionFeat
{
    public override string PromotionClassId => Constants.SharpshooterId;
    public override int PromotionClassFeatId => Constants.FeatSharpshooter;

    public override List<int> PromotionAbilities => new List<int>
    {
        Constants.StOverwatchId,
        Constants.ActChargedShotId,
        Constants.ActMarkHostilesId
    };

    protected override void ApplyInternalNPC(Chara c)
    {
        c.ability.Add(Constants.ActChargedShotId, 60, false);
        c.ability.Add(Constants.ActMarkHostilesId, 80, false);
        c.ability.Add(Constants.StOverwatchId, 100, false);
    }

    protected override bool Requirement()
    {
        return owner.Chara?.c_idJob == "gunner";
    }

    protected override void ApplyInternal()
    {
        // Gunning
        // Marksman
        // Stealth
        //owner.Chara.elements.ModPotential(286, 30);
    }
}
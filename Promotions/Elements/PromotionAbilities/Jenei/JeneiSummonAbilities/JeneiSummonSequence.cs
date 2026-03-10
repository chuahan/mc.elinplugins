using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Trait;
namespace PromotionMod.Elements.PromotionAbilities.Jenei.JeneiSummonAbilities;

public abstract class JeneiSummonSequence
{
    public virtual float SummonMultiplier => 1;

    // Jenei damage is calculated via:
    // distanceRatio - Depending on the distance from the Summon, they will take less damage.
    //                 Most summons have a Radius of 5.
    // base power + (SummonMultiplier * Target's Current HP)
    public int CalculateDamage(int power, int distance, Chara target)
    {
        float distanceRatio = distance switch
        {
            0 => 1f,
            1 => 1f,
            2 => 1f,
            3 => 0.7f,
            4 => 0.7f,
            _ => 0.4f
        };
        return (int)(distanceRatio * (power + target.MaxHP * SummonMultiplier));
    }

    public abstract bool PerformSummonAttack(Chara cc, int power);

    public virtual void SummonAnimation(Point pos)
    {
        // Make the Sprite Appear.
        // Animate the Sprite above the target and move them down to the Charas position.
    }
}
using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Trait;
namespace PromotionMod.Elements.PromotionAbilities.Jenei.JeneiSummonAbilities;

public class ActJeneiSummonSequence : Ability
{
    public virtual float SummonMultiplier => 1;
    public virtual float ManaCost => 0.3F;

    public override bool CanPerform()
    {
        if (!(CC.trait is TraitUniqueSummon)) return false;
        return base.CanPerform();
    }

    public override Cost GetCost(Chara c)
    {
        return new Cost
        {
            type = CostType.MP,
            cost = (int)(c.mana.max * ManaCost)
        };
    }

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
}
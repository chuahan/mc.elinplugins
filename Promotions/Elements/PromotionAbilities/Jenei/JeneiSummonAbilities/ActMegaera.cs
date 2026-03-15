using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Stats;
namespace PromotionMod.Elements.PromotionAbilities.Jenei.JeneiSummonAbilities;

/// <summary>
///     Fire. Sword Rain. Boosts all allies attack power by 12.5%
/// </summary>
public class ActMegaera : JeneiSummonSequence
{
    public override float SummonMultiplier => 0.06F;

    public override bool PerformSummonAttack(Chara cc, int power)
    {
        (List<Chara> friendlies, List<Chara> enemies) = HelperFunctions.GetOrganizedCharasWithinRadius(cc.pos, 5F, cc, true);
        for (int i = 0; i < enemies.Count; i++)
        {
            // TODO: Animate Swords falling.
            enemies[i].PlaySound("critical");
            enemies[i].PlayEffect("hit_slash").SetScale(1f);

            // Do Damage.
            int damage = CalculateDamage(power, enemies[i].pos.Distance(cc.pos), enemies[i]);
            HelperFunctions.ProcSpellDamage(power, damage, cc, enemies[i], ele: Constants.EleFire);
        }

        foreach (Chara ally in friendlies)
        {
            // TODO: Animate Swords Crossing.
            ally.AddCondition<ConAttackBoost>(15);
        }

        return true;
    }
}
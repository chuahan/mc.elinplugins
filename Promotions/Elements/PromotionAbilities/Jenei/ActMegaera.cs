using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Patches;
using PromotionMod.Stats;
namespace PromotionMod.Elements.PromotionAbilities.Jenei;

/// <summary>
/// Fire. Sword Rain. Boosts all allies attack power by 12.5%
/// </summary>
public class ActMegaera : ActJeneiSummonSequence
{
    public override float SummonMultiplier => 0.06F;
    
    public override bool Perform()
    {
        (List<Chara> friendlies, List<Chara> enemies) = HelperFunctions.GetOrganizedCharasWithinRadius(CC.pos, 5F, CC, true);
        for (int i = 0; i < enemies.Count; i++)
        {
            TC.PlaySound("critical");
            TC.PlayEffect("hit_slash").SetScale(1f);

            // Do Damage.
            int damage = this.CalculateDamage(this.GetPower(CC), enemies[i].pos.Distance(CC.pos), enemies[i]);
            HelperFunctions.ProcSpellDamage(this.GetPower(CC), damage, CC, TC.Chara, ele: Constants.EleFire);
        }

        foreach (Chara ally in friendlies)
        {
            ally.AddCondition<ConAttackBoost>(15);
        }
        
        return true;
    }
}
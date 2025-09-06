using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Patches;
namespace PromotionMod.Elements.PromotionAbilities.Jenei;

/// <summary>
/// Impact. Inflicts heavy poison.
/// </summary>
public class ActHaures : ActJeneiSummonSequence
{
    public override float SummonMultiplier => 0.15F;
    
    public override bool Perform()
    {
        List<Chara> targets = HelperFunctions.GetCharasWithinRadius(CC.pos, 5F, CC, false, true);
        for (int i = 0; i < targets.Count; i++)
        {
            TC.PlaySound("critical");
            TC.PlayEffect("hit_slash").SetScale(1f);

            // Do Damage.
            int damage = this.CalculateDamage(this.GetPower(CC), targets[i].pos.Distance(CC.pos), targets[i]);
            HelperFunctions.ProcSpellDamage(this.GetPower(CC), damage, CC, TC.Chara, ele: Constants.EleImpact);

            // Apply 20 Turns of Poison.
            if (targets[i].IsAliveInCurrentZone) 
            {
                targets[i].AddCondition<ConPoison>(80, force: true);
            }
        }

        return true;
    }
}
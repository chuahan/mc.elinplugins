using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Patches;
namespace PromotionMod.Elements.PromotionAbilities.Jenei;

/// <summary>
/// Lightning. Inflicts Sleep. Petal Rain.
/// </summary>
public class ActFlora : ActJeneiSummonSequence
{
    public override float SummonMultiplier => 0.09F;
    
    public override bool Perform()
    {
        List<Chara> targets = HelperFunctions.GetCharasWithinRadius(CC.pos, 5F, CC, false, true);
        for (int i = 0; i < targets.Count; i++)
        {
            TC.PlaySound("critical");
            TC.PlayEffect("hit_slash").SetScale(1f);

            // Do Damage.
            int damage = this.CalculateDamage(this.GetPower(CC), targets[i].pos.Distance(CC.pos), targets[i]);
            HelperFunctions.ProcSpellDamage(this.GetPower(CC), damage, CC, TC.Chara, element: Constants.EleLightning);

            if (EClass.rnd(2) == 0 && targets[i].IsAliveInCurrentZone) 
            {
                targets[i].AddCondition<ConSleep>(100, force: true);
            }
        }

        return true;
    }
}
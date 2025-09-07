using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Jenei.JeneiSummonAbilities;

/// <summary>
///     Lightning. Inflicts Sleep. Petal Rain.
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
            int damage = CalculateDamage(GetPower(CC), targets[i].pos.Distance(CC.pos), targets[i]);
            HelperFunctions.ProcSpellDamage(GetPower(CC), damage, CC, TC.Chara, ele: Constants.EleLightning);

            if (EClass.rnd(2) == 0 && targets[i].IsAliveInCurrentZone)
            {
                targets[i].AddCondition<ConSleep>(100, true);
            }
        }

        return true;
    }
}
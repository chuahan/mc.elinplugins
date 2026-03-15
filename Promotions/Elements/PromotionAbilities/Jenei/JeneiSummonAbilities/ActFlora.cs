using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Jenei.JeneiSummonAbilities;

/// <summary>
///     Lightning. Inflicts Sleep. Petal Rain.
/// </summary>
public class ActFlora : JeneiSummonSequence
{
    public override float SummonMultiplier => 0.09F;

    public override bool PerformSummonAttack(Chara cc, int power)
    {
        List<Chara> targets = HelperFunctions.GetCharasWithinRadius(cc.pos, 5F, cc, false, true);
        bool originalWasBlossom = EClass._map.config.blossom;
        EClass._map.config.blossom = true;
        for (int i = 0; i < targets.Count; i++)
        {
            targets[i].PlaySound("critical");
            targets[i].PlayEffect("hit_slash").SetScale(1f);

            // Do Damage.
            int damage = CalculateDamage(power, targets[i].pos.Distance(cc.pos), targets[i]);
            HelperFunctions.ProcSpellDamage(power, damage, cc, targets[i], ele: Constants.EleLightning);

            if (EClass.rnd(2) == 0 && targets[i].IsAliveInCurrentZone)
            {
                targets[i].AddCondition<ConSleep>(100, true);
            }
        }

        TweenUtil.Delay(1F, delegate
        {
            EClass._map.config.blossom = originalWasBlossom;
        });

        return true;
    }
}
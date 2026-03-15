using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Jenei.JeneiSummonAbilities;

/// <summary>
///     Impact. Inflicts heavy poison.
/// </summary>
public class ActHaures : JeneiSummonSequence
{
    public override float SummonMultiplier => 0.15F;

    public override bool PerformSummonAttack(Chara cc, int power)
    {
        List<Chara> targets = HelperFunctions.GetCharasWithinRadius(cc.pos, 5F, cc, false, true);
        for (int i = 0; i < targets.Count; i++)
        {
            targets[i].PlaySound("critical");
            targets[i].PlayEffect("hit_slash").SetScale(1f);

            // Do Damage.
            int damage = CalculateDamage(power, targets[i].pos.Distance(cc.pos), targets[i]);
            HelperFunctions.ProcSpellDamage(power, damage, cc, targets[i], ele: Constants.EleImpact);

            // Apply 20 Turns of Poison.
            if (targets[i].IsAliveInCurrentZone)
            {
                targets[i].AddCondition<ConPoison>(80, true);
            }
        }

        return true;
    }
}
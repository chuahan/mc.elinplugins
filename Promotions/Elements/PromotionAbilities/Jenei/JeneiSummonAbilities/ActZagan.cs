using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Stats;
namespace PromotionMod.Elements.PromotionAbilities.Jenei.JeneiSummonAbilities;

/// <summary>
///     Impact. Explosive axe strike. Applies PV Break of 50% to all enemies.
/// </summary>
public class ActZagan: JeneiSummonSequence
{
    public override float SummonMultiplier => 0.06F;

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

            if (targets[i].IsAliveInCurrentZone)
            {
                targets[i].AddCondition(SubPoweredCondition.Create(nameof(ConArmorBreak), power, 50));
            }
        }

        return true;
    }
}
using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Stats;
namespace PromotionMod.Elements.PromotionAbilities.Jenei.JeneiSummonAbilities;

/// <summary>
///     Impact. Explosive axe strike. Applies PV Break of 50% to all enemies.
/// </summary>
public class ActZagan : ActJeneiSummonSequence
{
    public override float SummonMultiplier => 0.06F;

    public override bool Perform()
    {
        List<Chara> targets = HelperFunctions.GetCharasWithinRadius(CC.pos, 5F, CC, false, true);
        for (int i = 0; i < targets.Count; i++)
        {
            TC.PlaySound("critical");
            TC.PlayEffect("hit_slash").SetScale(1f);

            // Do Damage.
            int damage = CalculateDamage(GetPower(CC), targets[i].pos.Distance(CC.pos), targets[i]);
            HelperFunctions.ProcSpellDamage(GetPower(CC), damage, CC, TC.Chara, ele: Constants.EleImpact);

            if (targets[i].IsAliveInCurrentZone)
            {
                targets[i].AddCondition(SubPoweredCondition.Create(nameof(ConArmorBreak), GetPower(CC), 50));
            }
        }

        return true;
    }
}
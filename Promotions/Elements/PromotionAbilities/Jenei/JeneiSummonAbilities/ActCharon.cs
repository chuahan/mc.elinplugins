using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Jenei.JeneiSummonAbilities;

/// <summary>
///     Impact. 5% chance of instantly killing target.
/// </summary>
public class ActCharon : ActJeneiSummonSequence
{
    public override float SummonMultiplier => 0.3F;

    public override bool Perform()
    {
        List<Chara> targets = HelperFunctions.GetCharasWithinRadius(CC.pos, 5F, CC, false, true);
        for (int i = 0; i < targets.Count; i++)
        {
            // SFX: Nether ball.
            Effect spellEffect = Effect.Get("Element/ball_Nether");
            spellEffect.Play(CC.pos, 0f, targets[i].pos);

            // Do Damage.
            int damage = CalculateDamage(GetPower(CC), targets[i].pos.Distance(CC.pos), targets[i]);
            HelperFunctions.ProcSpellDamage(GetPower(CC), damage, CC, TC.Chara, ele: Constants.EleImpact);

            // 1/20 chance of Instakill
            if (EClass.rnd(20) == 0 && targets[i].IsAliveInCurrentZone)
            {
                targets[i].DamageHP(targets[i].MaxHP, AttackSource.Finish, CC);
            }
        }

        return true;
    }
}
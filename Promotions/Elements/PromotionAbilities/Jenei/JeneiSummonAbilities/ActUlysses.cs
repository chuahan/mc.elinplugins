using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Jenei.JeneiSummonAbilities;

/// <summary>
///     Fire. Guaranteed unconscious affliction.
/// </summary>
public class ActUlysses : ActJeneiSummonSequence
{
    public override float SummonMultiplier => 0.12F;

    public override bool Perform()
    {
        List<Chara> targets = HelperFunctions.GetCharasWithinRadius(CC.pos, 5F, CC, false, true);
        for (int i = 0; i < targets.Count; i++)
        {
            // SFX: Nether ball.
            Effect spellEffect = Effect.Get("Element/ball_Magic");
            spellEffect.Play(CC.pos, 0f, targets[i].pos);

            // Do Damage.
            int damage = CalculateDamage(GetPower(CC), targets[i].pos.Distance(CC.pos), targets[i]);
            HelperFunctions.ProcSpellDamage(GetPower(CC), damage, CC, TC.Chara, ele: Constants.EleFire);

            if (targets[i].IsAliveInCurrentZone) targets[i].AddCondition<ConFaint>(force: true);
        }

        return true;
    }
}
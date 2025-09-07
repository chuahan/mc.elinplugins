using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Jenei.JeneiSummonAbilities;

/// <summary>
///     Cold Damage.
/// </summary>
public class ActBoreas : ActJeneiSummonSequence
{
    public override float SummonMultiplier => 0.12F;

    public override bool Perform()
    {
        List<Chara> targets = HelperFunctions.GetCharasWithinRadius(CC.pos, 5F, CC, false, true);
        for (int i = 0; i < targets.Count; i++)
        {
            Effect spellEffect = Effect.Get("Element/ball_Cold");
            spellEffect.Play(CC.pos, 0f, targets[i].pos);

            int damage = CalculateDamage(GetPower(CC), targets[i].pos.Distance(CC.pos), targets[i]);
            HelperFunctions.ProcSpellDamage(GetPower(CC), damage, CC, TC.Chara, ele: Constants.EleCold);
        }

        return true;
    }
}
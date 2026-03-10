using System.Collections.Generic;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Jenei.JeneiSummonAbilities;

/// <summary>
///     Impact damage.
/// </summary>
public class ActCybele: JeneiSummonSequence
{
    public override float SummonMultiplier => 0.12F;

    public override bool PerformSummonAttack(Chara cc, int power)
    {
        // Play Earthquake effect:
        Effect effect = null;
        Point from = cc.pos;

        if (EClass.rnd(4) == 0 && from.IsSync)
        {
            effect = Effect.Get("smoke_earthquake");
        }
        float num3 = 0.06f * cc.pos.Distance(from);
        Point pos = from.Copy();

        TweenUtil.Tween(num3, null, delegate
        {
            pos.Animate(AnimeID.Quake, true);
        });
        effect?.SetStartDelay(num3);
        cc.PlaySound("spell_earthquake");
        Shaker.ShakeCam("ball");

        List<Chara> targets = HelperFunctions.GetCharasWithinRadius(cc.pos, 5F, cc, false, true);
        for (int i = 0; i < targets.Count; i++)
        {
            // Do Damage.
            int damage = CalculateDamage(power, targets[i].pos.Distance(cc.pos), targets[i]);
            HelperFunctions.ProcSpellDamage(power, damage, cc, targets[i], ele: Constants.EleImpact);
        }

        return true;
    }
}
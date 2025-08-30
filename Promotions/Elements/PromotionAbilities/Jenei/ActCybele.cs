using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Patches;
namespace PromotionMod.Elements.PromotionAbilities.Jenei;

/// <summary>
/// Impact damage.
/// </summary>
public class ActCybele : ActJeneiSummonSequence
{
    public override float SummonMultiplier => 0.12F;
    
    public override bool Perform()
    {
        // Play Earthquake effect:
        Effect effect = null;
        Point from = CC.pos;

        if (EClass.rnd(4) == 0 && from.IsSync)
        {
            effect = Effect.Get("smoke_earthquake");
        }
        float num3 = 0.06f * CC.pos.Distance(from);
        Point pos = from.Copy();

        TweenUtil.Tween(num3, null, delegate
        {
            pos.Animate(AnimeID.Quake, true);
        });
        effect?.SetStartDelay(num3);
        CC.PlaySound("spell_earthquake");
        Shaker.ShakeCam("ball");
        
        List<Chara> targets = HelperFunctions.GetCharasWithinRadius(CC.pos, 5F, CC, false, true);
        for (int i = 0; i < targets.Count; i++)
        {
            // Do Damage.
            int damage = this.CalculateDamage(this.GetPower(CC), targets[i].pos.Distance(CC.pos), targets[i]);
            HelperFunctions.ProcSpellDamage(this.GetPower(CC), damage, CC, TC.Chara, element: Constants.EleImpact);
        }
        
        return true;
    }
}
using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Jenei.JeneiSummonAbilities;

/// <summary>
///     Fire. Guaranteed unconscious affliction.
/// </summary>
public class ActUlysses: JeneiSummonSequence
{
    public override float SummonMultiplier => 0.12F;

    public override bool PerformSummonAttack(Chara cc, int power)
    {
        cc.PlaySound("curse");
        ElementRef colorRef = EClass.setting.elements["eleNether"];
        List<Chara> targetsHit = new List<Chara>();
        foreach (Point tile in EClass._map.ListPointsInCircle(cc.pos, 5f, false, true))
        {
            int distance = tile.Distance(cc.pos);
            foreach (Chara target in tile.ListCharas().Where(target => !targetsHit.Contains(target) && target.IsHostile(cc)))
            {
                // Do Damage.
                int damage = CalculateDamage(power, target.pos.Distance(cc.pos), target);
                HelperFunctions.ProcSpellDamage(power, damage, cc, target, ele: Constants.EleFire);

                if (target.IsAliveInCurrentZone) target.AddCondition<ConFaint>(force: true);

                // Mark Target as hit.
                targetsHit.Add(target);
            }

            // Get distance from the origin. Use that to add delay to the explosion.
            Effect spellEffect = Effect.Get("Element/ball_Magic");
            spellEffect.SetParticleColor(colorRef.colorTrail, true, "_TintColor");
            spellEffect.sr.color = colorRef.colorSprite;
            float delay = distance * 0.08F;
            spellEffect.SetStartDelay(delay);
            spellEffect.Play(tile).Flip(tile.x > cc.pos.x);
        }
        
        return true;
    }
}
using System.Collections.Generic;
using System.Linq;
using Cwl.Helper.Extensions;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Jenei.JeneiSummonAbilities;

/// <summary>
///     Impact. 5% chance of instantly killing target.
/// </summary>
public class ActCharon : JeneiSummonSequence
{
    public override float SummonMultiplier => 0.3F;

    public override bool PerformSummonAttack(Chara cc, int power)
    {
        cc.PlaySound("spell_breathe");
        ElementRef colorRef = EClass.setting.elements["eleNether"];
        List<Chara> targetsHit = new List<Chara>();
        foreach (Point tile in EClass._map.ListPointsInCircle(cc.pos, 5f, false, false))
        {
            int distance = tile.Distance(cc.pos);
            foreach (Chara target in tile.ListCharas().Where(target => !targetsHit.Contains(target) && target.IsHostile(cc)))
            {
                // Do Damage.
                int damage = CalculateDamage(power, target.pos.Distance(cc.pos), target);
                HelperFunctions.ProcSpellDamage(power, damage, cc, target, ele: Constants.EleImpact);

                // 1/20 chance of Instakill. Does not work on bosses.
                if (EClass.rnd(20) == 0 && target.IsAliveInCurrentZone && !target.IsBoss())
                {
                    target.DamageHP(target.MaxHP, AttackSource.Finish, cc);
                }

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
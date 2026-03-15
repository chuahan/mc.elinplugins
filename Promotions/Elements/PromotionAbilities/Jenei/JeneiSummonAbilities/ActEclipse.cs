using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Stats;
namespace PromotionMod.Elements.PromotionAbilities.Jenei.JeneiSummonAbilities;

/// <summary>
///     Lightning. Reduces enemy attack by 50%.
/// </summary>
public class ActEclipse : JeneiSummonSequence
{
    public override float SummonMultiplier => 0.15F;


    public override bool PerformSummonAttack(Chara cc, int power)
    {
        // SFX: Cast holy light on self. Earthquake. Explode out in a lightning explosion
        Effect laser = Effect.Get("aura_heaven");
        laser.Play(cc.pos);

        // Play Earthquake at the same time as the element boom. The explosion goes up around 2F delay.
        TweenUtil.Delay(0.08F, delegate
        {
            Effect effect = null;
            Point from = cc.pos;
            if (EClass.rnd(4) == 0 && from.IsSync) effect = Effect.Get("smoke_earthquake");
            float num3 = 0.06f * cc.pos.Distance(from);
            Point pos = from.Copy();
            TweenUtil.Tween(num3, null, delegate
            {
                pos.Animate(AnimeID.Quake, true);
            });
            if (effect != null) effect.SetStartDelay(num3);
            cc.PlaySound("spell_earthquake");
            Shaker.ShakeCam("ball");
        });

        List<Chara> targetsHit = new List<Chara>();
        foreach (Point tile in EClass._map.ListPointsInCircle(cc.pos, 5f, false, false))
        {
            int distance = tile.Distance(cc.pos);
            foreach (Chara target in tile.ListCharas().Where(target => !targetsHit.Contains(target) && target.IsHostile(cc)))
            {
                // Do Damage.
                int damage = CalculateDamage(power, target.pos.Distance(cc.pos), target);
                HelperFunctions.ProcSpellDamage(power, damage, cc, target, ele: Constants.EleLightning);

                if (target.IsAliveInCurrentZone) target.AddCondition<ConAttackBreak>(50, true);

                // Mark Target as hit.
                targetsHit.Add(target);
            }

            // Get distance from the origin. Use that to add delay to the explosion.
            Effect spellEffect = Effect.Get("Element/ball_Magic");
            float delay = distance * 0.08F;
            spellEffect.SetStartDelay(delay);
            spellEffect.Play(tile).Flip(tile.x > cc.pos.x);

        }
        return true;
    }
}
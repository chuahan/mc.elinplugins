using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Jenei.JeneiSummonAbilities;

/// <summary>
///     Lightning. Reduces their mana and stamina by 10%.
/// </summary>
public class ActCatastrophe : JeneiSummonSequence
{
    public override float SummonMultiplier => 0.24F;

    public override bool PerformSummonAttack(Chara cc, int power)
    {
        // SFX: Cast holy light on self. Earthquake. Then send out lightning beams in all directions.
        Effect laser = Effect.Get("aura_heaven");
        ElementRef blueRef = EClass.setting.elements["eleCold"];
        ElementRef colorRef = EClass.setting.elements["eleChaos"];
        laser.SetParticleColor(blueRef.colorTrail, true, "_TintColor");
        laser.sr.color = blueRef.colorSprite;
        laser.Play(cc.pos);
        TweenUtil.Delay(0.08F, delegate
        {
            // Play Earthquake
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
                // Damage Target
                int damage = CalculateDamage(power, distance, target);
                HelperFunctions.ProcSpellDamage(power, damage, cc, target, ele: Constants.EleLightning);

                // Do Mana and Stamina Damage.
                if (!target.IsAliveInCurrentZone) continue;
                target.mana.Mod((int)(target.mana.max * 0.1F));
                target.stamina.Mod((int)(target.stamina.max * 0.1F));

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
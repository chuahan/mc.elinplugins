using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Patches;
using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.Jenei;

/// <summary>
/// Lightning. Reduces their mana and stamina by 10%.
/// </summary>
public class ActCatastrophe : ActJeneiSummonSequence
{
    public override float SummonMultiplier => 0.24F;
    
    public override bool Perform()
    {
        // SFX: Cast holy light on self. Earthquake. Then send out lightning beams in all directions.
        Effect laser = Effect.Get("aura_heaven");
        ElementRef blueRef = setting.elements["eleCold"];
        laser.SetParticleColor(blueRef.colorTrail, changeMaterial: true, "_TintColor");
        laser.sr.color = blueRef.colorSprite;
        laser.Play(CC.pos);
        TweenUtil.Delay(0.7F, delegate
        {
            // Play Earthquake
            Effect effect = null;
            Point from = CC.pos;
            if (EClass.rnd(4) == 0 && from.IsSync) effect = Effect.Get("smoke_earthquake");
            float num3 = 0.06f * CC.pos.Distance(from);
            Point pos = from.Copy();
            TweenUtil.Tween(num3, null, delegate
            {
                pos.Animate(AnimeID.Quake, true);
            });
            if (effect != null) effect.SetStartDelay(num3);
            CC.PlaySound("spell_earthquake");
            Shaker.ShakeCam("ball");
        });
        
        Effect spellEffect = Effect.Get("Element/ball_Chaos");
        List<Chara> targetsHit = new List<Chara>();
        foreach (Point tile in EClass._map.ListPointsInCircle(CC.pos, 5f, mustBeWalkable: false, los:false))
        {
            int distance = tile.Distance(CC.pos);
            foreach (Chara target in tile.ListCharas().Where(target => !targetsHit.Contains(target) && target.IsHostile(CC)))
            {
                // Damage Target
                int damage = this.CalculateDamage(this.GetPower(CC), distance, target);
                HelperFunctions.ProcSpellDamage(this.GetPower(CC), damage, CC, TC.Chara, ele: Constants.EleLightning);
                    
                // Do Mana and Stamina Damage.
                if (!target.IsAliveInCurrentZone) continue;
                target.mana.Mod((int)(target.mana.max * 0.1F));
                target.stamina.Mod((int)(target.stamina.max * 0.1F));
                    
                // Mark Target as hit.
                targetsHit.Add(target);
            }
            
            // Get distance from the origin. Use that to add delay to the explosion,
            float delay = distance * 0.7F;
            TweenUtil.Delay(delay, delegate
            {
                spellEffect.Play(tile, 0f, tile);
            });
        }
        
        return true;
    }
}
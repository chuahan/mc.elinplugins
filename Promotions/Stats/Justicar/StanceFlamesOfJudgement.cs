using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
using PromotionMod.Stats.Spellblade;
using UnityEngine;
namespace PromotionMod.Stats.Justicar;

public class StanceFlamesOfJudgement : BaseStance
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }

    public override void Tick()
    {
        if (_zone.IsRegion)
        {
            // Not allowed in regions.
            Kill();
        }

        if (owner.hp <= owner.MaxHP * 0.3F)
        {
            // TODO Text: Stance off.
            Kill();
        }
        
        List<Chara> targetsHit = new List<Chara>();
        // Get Karma Scores for the Player.
        // NPCs will be considered 0 Karma.
        bool positiveKarma = false, negativeKarma = false;
        if (CC.IsPCFactionOrMinion || CC.IsPC)
        {
            // For PC Faction, use PC's Karma.
            positiveKarma = player.karma >= 0;
            negativeKarma = player.karma < 0;
        }

        ElementRef colorRef = EClass.setting.elements["eleFire"];
        if (negativeKarma)
        {
            colorRef = EClass.setting.elements["eleFire"];
        } else if (positiveKarma)
        {
            colorRef = EClass.setting.elements["eleHoly"];
        }
            
        int firePower = (int)(owner.MaxHP * 0.3F);
        
        // Do self-damage.
        HelperFunctions.ProcSpellDamage(power, firePower, owner, owner, AttackSource.None, Constants.EleFire, 50);
        
        foreach (Point tile in EClass._map.ListPointsInCircle(CC.pos, 5f, false, false))
        {
            int distance = tile.Distance(CC.pos);
            foreach (Chara target in tile.ListCharas().Where(target => !targetsHit.Contains(target)))
            {
                if (target.IsHostile(CC))
                {
                    HelperFunctions.ProcSpellDamage(power, firePower, owner, target, AttackSource.None, Constants.EleFire, 50);
                    if (negativeKarma) target.AddCondition(SubPoweredCondition.Create(nameof(ConFireBreak), power, 10));
                }
                
                if (positiveKarma && !target.IsHostile(CC))
                {
                    target.HealHP(firePower / 2, HealSource.Magic);
                }

                // Mark Target as hit.
                targetsHit.Add(target);
            }

            // Get distance from the origin. Use that to add delay to the explosion.
            Effect spellEffect = Effect.Get("Element/ball_Fire");
            spellEffect.SetParticleColor(colorRef.colorTrail, true, "_TintColor");
            spellEffect.sr.color = colorRef.colorSprite;
            float delay = distance * 0.08F;
            spellEffect.SetStartDelay(delay);
            spellEffect.Play(tile).Flip(tile.x > CC.pos.x);
        }
    }
}
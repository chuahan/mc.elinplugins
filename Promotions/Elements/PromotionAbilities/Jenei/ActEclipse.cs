using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Patches;
using PromotionMod.Stats;
using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.Jenei;

/// <summary>
/// Lightning. Reduces enemy attack by 50%.
/// </summary>
public class ActEclipse : ActJeneiSummonSequence
{
    public override float SummonMultiplier => 0.15F;
    
    
    public override bool Perform()
    {
        // SFX: Cast holy light on self. Earthquake. Then send out lightning beams in all directions.
        Effect laser = Effect.Get("aura_heaven");
        ElementRef colorRef = setting.elements["eleChaos"];
        laser.SetParticleColor(colorRef.colorTrail, changeMaterial: true, "_TintColor");
        laser.sr.color = colorRef.colorSprite;
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
        
        List<Chara> targets = HelperFunctions.GetCharasWithinRadius(CC.pos, 5F, CC, false, true);
        for (int i = 0; i < targets.Count; i++)
        {
            Point from = CC.pos;
            ElementRef elementRef = setting.elements["eleLightning"];
            Effect spellEffect = Effect.Get("trail1");
            spellEffect.SetParticleColor(elementRef.colorTrail, changeMaterial: true, "_TintColor").Play(from);
            spellEffect.sr.color = elementRef.colorSprite;
            TrailRenderer componentInChildren = spellEffect.GetComponentInChildren<TrailRenderer>();
            Color startColor = componentInChildren.endColor = elementRef.colorSprite;
            componentInChildren.startColor = startColor;
            spellEffect.Play(CC.pos, 0f, targets[i].pos);
            
            // Do Damage.
            int damage = this.CalculateDamage(this.GetPower(CC), targets[i].pos.Distance(CC.pos), targets[i]);
            HelperFunctions.ProcSpellDamage(this.GetPower(CC), damage, CC, TC.Chara, element: Constants.EleLightning);

            // Apply Attack Down.
            if (targets[i].IsAliveInCurrentZone) targets[i].AddCondition<ConAttackBreak>(50, force: true);
        }
        
        return true;
    }
}
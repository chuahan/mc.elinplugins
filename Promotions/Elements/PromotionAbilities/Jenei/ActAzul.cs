using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Patches;
using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.Jenei;

/// <summary>
/// Cold. Inflicts Stun.
/// </summary>
public class ActAzul : ActJeneiSummonSequence
{
    public override float SummonMultiplier => 0.21F;
    
    public override bool Perform()
    {
        List<Chara> targets = HelperFunctions.GetCharasWithinRadius(CC.pos, 5F, CC, false, true);
        for (int i = 0; i < targets.Count; i++)
        {
            // SFX: Water beams at all targets.
            Point from = CC.pos;
            ElementRef elementRef = setting.elements["eleCold"];
            Effect spellEffect = Effect.Get("trail1");
            spellEffect.SetParticleColor(elementRef.colorTrail, changeMaterial: true, "_TintColor").Play(from);
            spellEffect.sr.color = elementRef.colorSprite;
            TrailRenderer componentInChildren = spellEffect.GetComponentInChildren<TrailRenderer>();
            Color startColor = componentInChildren.endColor = elementRef.colorSprite;
            componentInChildren.startColor = startColor;
            spellEffect.Play(CC.pos, 0f, targets[i].pos);
            
            HelperFunctions.ProcSpellDamage(this.GetPower(CC), this.CalculateDamage(this.GetPower(CC), targets[i].pos.Distance(CC.pos), targets[i]), CC, TC.Chara, element: Constants.EleCold);

            // Inflict Stun. 1/5 chance to guarantee.
            if (EClass.rnd(5) == 0 && targets[i].IsAliveInCurrentZone)
            {
                targets[i].AddCondition<ConParalyze>(100, true);
            }
        }
        
        return true;
    }
}
using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Patches;
using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.Jenei;

/// <summary>
/// Lightning damage.
/// Fires green arrows at all enemies. Atalanta has no distance penalty due to arrows.
/// </summary>
public class ActAtalanta : ActJeneiSummonSequence
{
    public override float SummonMultiplier => 0.12F;
    
    public override bool Perform()
    {
        List<Chara> targets = HelperFunctions.GetCharasWithinRadius(CC.pos, 5F, CC, false, true);
        for (int i = 0; i < targets.Count; i++)
        {
            // SFX: Fire acid arrow at all targets (cause green)
            ElementRef elementRef = setting.elements["eleAcid"];
            Effect arrowEffect = Effect.Get("spell_arrow");
            arrowEffect.sr.color = elementRef.colorSprite;
            TrailRenderer componentInChildren = arrowEffect.GetComponentInChildren<TrailRenderer>();
            Color startColor = componentInChildren.endColor = elementRef.colorSprite;
            componentInChildren.startColor = startColor;
            arrowEffect.Play(CC.pos, 0f, targets[i].pos);

            int damage = this.CalculateDamage(this.GetPower(CC), 0, targets[i]);
            HelperFunctions.ProcSpellDamage(this.GetPower(CC), damage, CC, TC.Chara, ele: Constants.EleLightning);
        }
        
        return true;
    }
}
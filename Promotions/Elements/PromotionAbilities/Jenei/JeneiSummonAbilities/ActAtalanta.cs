using System.Collections.Generic;
using PromotionMod.Common;
using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.Jenei.JeneiSummonAbilities;

/// <summary>
///     Lightning damage.
///     Fires green arrows at all enemies. Atalanta has no distance penalty due to arrows.
/// </summary>
public class ActAtalanta : JeneiSummonSequence
{
    public override float SummonMultiplier => 0.12F;

    public override bool PerformSummonAttack(Chara cc, int power)
    {
        List<Chara> targets = HelperFunctions.GetCharasWithinRadius(cc.pos, 5F, cc, false, true);

        // Acid Arrow for Green.
        ElementRef elementRef = EClass.setting.elements["eleAcid"];
        Effect arrowEffect = Effect.Get("spell_arrow");
        arrowEffect.sr.color = elementRef.colorSprite;
        TrailRenderer componentInChildren = arrowEffect.GetComponentInChildren<TrailRenderer>();
        Color startColor = componentInChildren.endColor = elementRef.colorSprite;
        componentInChildren.startColor = startColor;

        for (int i = 0; i < targets.Count; i++)
        {
            arrowEffect.Play(cc.pos, 0f, targets[i].pos);
            int damage = CalculateDamage(power, 0, targets[i]);
            HelperFunctions.ProcSpellDamage(power, damage, cc, targets[i], ele: Constants.EleLightning);
        }

        return true;
    }
}
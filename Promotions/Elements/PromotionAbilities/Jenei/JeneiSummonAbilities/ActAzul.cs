using System.Collections.Generic;
using PromotionMod.Common;
using UnityEngine;
namespace PromotionMod.Elements.PromotionAbilities.Jenei.JeneiSummonAbilities;

/// <summary>
///     Cold. Inflicts Stun.
/// </summary>
public class ActAzul : JeneiSummonSequence
{
    public override float SummonMultiplier => 0.21F;

    public override bool PerformSummonAttack(Chara cc, int power)
    {
        List<Chara> targets = HelperFunctions.GetCharasWithinRadius(cc.pos, 5F, cc, false, true);

        // SFX: Water beams at all targets.
        Point from = cc.pos;
        ElementRef elementRef = EClass.setting.elements["eleCold"];
        Effect spellEffect = Effect.Get("trail1");
        spellEffect.SetParticleColor(elementRef.colorTrail, true, "_TintColor").Play(from);
        spellEffect.sr.color = elementRef.colorSprite;
        TrailRenderer componentInChildren = spellEffect.GetComponentInChildren<TrailRenderer>();
        Color startColor = componentInChildren.endColor = elementRef.colorSprite;
        componentInChildren.startColor = startColor;

        for (int i = 0; i < targets.Count; i++)
        {
            spellEffect.Play(cc.pos, 0f, targets[i].pos);

            HelperFunctions.ProcSpellDamage(
                power,
                CalculateDamage(
                    power,
                    targets[i].pos.Distance(cc.pos),
                    targets[i]),
                cc, targets[i], ele: Constants.EleCold);

            // Inflict Stun. 1/5 chance to guarantee.
            if (EClass.rnd(5) == 0 && targets[i].IsAliveInCurrentZone)
            {
                targets[i].AddCondition<ConParalyze>(100, true);
                // Blast the enemy away from you, like hydropump.
                List<Point> linePoints = EClass._map.ListPointsInLine(EClass.pc.pos, targets[i].pos, 10);
                for (int j = linePoints.Count; j > 0; j--)
                {
                    if (!linePoints[j].HasChara && !linePoints[j].IsBlocked)
                    {
                        targets[i].MoveImmediate(linePoints[j]);
                        break;
                    }
                }
            }

        }

        return true;
    }
}
using System.Collections.Generic;
using System.Linq;
namespace PromotionMod.Elements.Alraune;

public class ActWildGrowth : Ability
{
    public override Cost GetCost(Chara c)
    {
        Cost convertToMp = base.GetCost(c);
        convertToMp.type = CostType.MP;
        return convertToMp;
    }

    public override bool Perform()
    {
        ElementRef colorRef = setting.elements["elePoison"];
        CC.PlaySound("whip");
        List<Chara> targetsHit = new List<Chara>();
        foreach (Point tile in _map.ListPointsInCircle(CC.pos, 3F, false, false))
        {
            int distance = tile.Distance(CC.pos);
            foreach (Chara target in tile.ListCharas().Where(target => !targetsHit.Contains(target) && target.IsHostile(CC)))
            {
                // Apply Entangle
                ActEffect.ProcAt(EffectId.Debuff, GetPower(CC), BlessedState.Normal, CC, target, target.pos, true, new ActRef
                {
                    origin = CC,
                    n1 = nameof(ConEntangle)
                });

                // Apply Poison
                ActEffect.ProcAt(EffectId.Debuff, GetPower(CC), BlessedState.Normal, CC, target, target.pos, true, new ActRef
                {
                    origin = CC,
                    n1 = nameof(ConPoison)
                });

                // Mark Target as hit.
                targetsHit.Add(target);
            }

            // Play Green Lightning effect.
            Effect spellEffect = Effect.Get("Element/ball_Lightning");
            spellEffect.SetParticleColor(colorRef.colorTrail, true, "_TintColor");
            spellEffect.sr.color = colorRef.colorSprite;
            float delay = distance * 0.08F;
            spellEffect.SetStartDelay(delay);
            spellEffect.Play(tile).Flip(tile.x > CC.pos.x);
        }

        return true;
    }
}
using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Jenei.JeneiSummonAbilities;

/// <summary>
///     Fire. Massive solar explosion that does full damage. Revives all allies. Restores all allies to full health.
/// </summary>
public class ActIris : JeneiSummonSequence
{
    public override float SummonMultiplier => 0.4F;

    public override bool PerformSummonAttack(Chara cc, int power)
    {
        ElementRef colorRef = EClass.setting.elements["eleLightning"];

        // Revive all fallen allies if Iris is allied to PC.
        if (cc.IsPCPartyMinion)
        {
            List<KeyValuePair<int, Chara>> deadAllies = EClass.game.cards.globalCharas
                    .Where(c => c.Value.isDead && c.Value.faction == EClass.pc.faction && !c.Value.isSummon && c.Value.c_wasInPcParty)
                    .ToList();
            foreach (KeyValuePair<int, Chara> c in deadAllies)
            {
                c.Value.Chara.GetRevived();
            }
        }

        List<Chara> targetsHit = new List<Chara>();
        foreach (Point tile in EClass._map.ListPointsInCircle(cc.pos, 5f, false, false))
        {
            int distance = tile.Distance(cc.pos);
            foreach (Chara target in tile.ListCharas())
            {
                if (!targetsHit.Contains(target))
                {
                    if (target.IsHostile(cc))
                    {
                        // Damage Target
                        int damage = CalculateDamage(power, distance, target);
                        HelperFunctions.ProcSpellDamage(power, damage, cc, target, ele: Constants.EleFire);
                    }
                    else
                    {
                        // Heal any allies
                        target.HealHP(target.MaxHP, HealSource.Magic);
                    }
                }

                targetsHit.Add(target);
            }

            // Get distance from the origin. Use that to add delay to the explosion,
            Effect spellEffect = Effect.Get("Element/ball_Fire");
            spellEffect.SetParticleColor(colorRef.colorTrail, true, "_TintColor");
            spellEffect.sr.color = colorRef.colorSprite;
            float delay = distance * 0.08F;
            spellEffect.SetStartDelay(delay);
            spellEffect.Play(tile).Flip(tile.x > cc.pos.x);
        }
        return true;
    }
}
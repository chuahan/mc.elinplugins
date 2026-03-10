using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Jenei.JeneiSummonAbilities;

/// <summary>
///     Cold breath. Inflicts -50% speed multiplier and -50% DV Multiplier.
/// </summary>
public class ActMoloch: JeneiSummonSequence
{
    public override float SummonMultiplier => 0.09F;

    public override bool PerformSummonAttack(Chara cc, int power)
    {
        cc.PlaySound("spell_breathe");
        List<Chara> targetsHit = new List<Chara>();
        foreach (Point tile in EClass._map.ListPointsInCircle(cc.pos, 5f, false, false))
        {
            int distance = tile.Distance(cc.pos);
            foreach (Chara target in tile.ListCharas().Where(target => !targetsHit.Contains(target)).Where(target => target.IsHostile(cc)))
            {
                // Damage Target
                int damage = CalculateDamage(power, distance, target);
                HelperFunctions.ProcSpellDamage(power, damage, cc, target, ele: Constants.EleCold);
                targetsHit.Add(target);
            }

            // Get distance from the origin. Use that to add delay to the explosion.
            Effect spellEffect = Effect.Get("Element/ball_Cold");
            float delay = distance * 0.08F;
            spellEffect.SetStartDelay(delay);
            spellEffect.Play(tile).Flip(tile.x > cc.pos.x);
        }

        return true;
    }
}
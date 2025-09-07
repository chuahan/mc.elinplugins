using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Jenei.JeneiSummonAbilities;

/// <summary>
///     Cold breath. Inflicts -50% speed multiplier and -50% DV Multiplier.
/// </summary>
public class ActMoloch : ActJeneiSummonSequence
{
    public override float SummonMultiplier => 0.09F;

    public override bool Perform()
    {
        CC.PlaySound("spell_breathe");
        Effect spellEffect = Effect.Get("Element/ball_Cold");
        List<Chara> targetsHit = new List<Chara>();
        foreach (Point tile in _map.ListPointsInCircle(CC.pos, 5f, false, false))
        {
            int distance = tile.Distance(CC.pos);
            foreach (Chara target in tile.ListCharas().Where(target => !targetsHit.Contains(target)).Where(target => target.IsHostile(CC)))
            {
                // Damage Target
                int damage = CalculateDamage(GetPower(CC), distance, target);
                HelperFunctions.ProcSpellDamage(GetPower(CC), damage, CC, TC.Chara, ele: Constants.EleCold);
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
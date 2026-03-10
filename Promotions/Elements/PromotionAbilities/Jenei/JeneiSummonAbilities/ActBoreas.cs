using System.Collections.Generic;
using System.Linq;
using PromotionMod.Common;
namespace PromotionMod.Elements.PromotionAbilities.Jenei.JeneiSummonAbilities;

/// <summary>
///     Cold Damage.
/// </summary>
public class ActBoreas: JeneiSummonSequence
{
    public override float SummonMultiplier => 0.12F;

    public override bool PerformSummonAttack(Chara cc, int power)
    {
        cc.PlaySound("spell_breathe");
        List<Chara> targetsHit = new List<Chara>();
        foreach (Point tile in EClass._map.ListPointsInCircle(cc.pos, 5f, false, false))
        {
            int distance = tile.Distance(cc.pos);
            foreach (Chara target in tile.ListCharas().Where(target => !targetsHit.Contains(target) && target.IsHostile(cc)))
            {
                // Do Damage.
                int damage = CalculateDamage(power, target.pos.Distance(cc.pos), target);
                HelperFunctions.ProcSpellDamage(power, damage, cc, target, ele: Constants.EleCold);

                // Mark Target as hit.
                targetsHit.Add(target);
            }

            Effect spellEffect = Effect.Get("Element/ball_Cold");
            float delay = distance * 0.08F;
            spellEffect.SetStartDelay(delay);
            spellEffect.Play(tile).Flip(tile.x > cc.pos.x);
        }

        return true;
    }
}
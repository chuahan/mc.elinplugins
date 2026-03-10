using System.Collections.Generic;
using System.Linq;
using Cwl.Helper.Extensions;
using PromotionMod.Common;
using PromotionMod.Stats.Jenei;
namespace PromotionMod.Elements.PromotionAbilities.Jenei.JeneiSummonAbilities;

/// <summary>
///     Cold. Doesn't do damage. Heals all allies. Applies healing over time doing 60%, 50%, 40%, 30%, and 20%.
/// </summary>
public class ActCoatilcue: JeneiSummonSequence
{
    public override float SummonMultiplier => 0.6F;
    
    public override bool PerformSummonAttack(Chara cc, int power)
    {
        cc.PlaySound("Footstep/water");
        List<Chara> targetsHit = new List<Chara>();
        foreach (Point tile in EClass._map.ListPointsInCircle(cc.pos, 10f, false, false))
        {
            int distance = tile.Distance(cc.pos);
            foreach (Chara target in tile.ListCharas().Where(target => !targetsHit.Contains(target) && !target.IsHostile(cc)))
            {
                // 30% Heal.
                target.HealHP((int)(target.MaxHP * 0.3F), HealSource.Magic);
                // Apply Coatilcue Healing
                target.AddCondition<ConWatersOfLife>(force: true);
                
                // Mark Target as hit.
                targetsHit.Add(target);
            }

            // Get distance from the origin. Use that to add delay to the explosion.
            float delay = distance * 0.08F;
            TweenUtil.Delay(delay, delegate
            {
                tile.PlayEffect("ripple");
            });
        }

        return true;
    }
}
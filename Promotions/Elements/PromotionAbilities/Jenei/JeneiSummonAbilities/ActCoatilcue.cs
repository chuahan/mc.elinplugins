using System.Collections.Generic;
using PromotionMod.Common;
using PromotionMod.Stats.Jenei;
namespace PromotionMod.Elements.PromotionAbilities.Jenei.JeneiSummonAbilities;

/// <summary>
///     Cold. Doesn't do damage. Heals all allies. Applies healing over time doing 60%, 50%, 40%, 30%, and 20%.
/// </summary>
public class ActCoatilcue : ActJeneiSummonSequence
{
    public override float SummonMultiplier => 0.6F;

    public override bool Perform()
    {
        List<Chara> targets = HelperFunctions.GetCharasWithinRadius(CC.pos, 10F, CC, true, false);
        for (int i = 0; i < targets.Count; i++)
        {
            // 30% Heal.
            targets[i].HealHP((int)(targets[i].MaxHP * 0.3F), HealSource.Magic);
            // Apply Coatilcue Healing
            targets[i].AddCondition<ConWatersOfLife>(force: true);
        }

        return true;
    }
}
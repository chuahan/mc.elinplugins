using System.Linq;
namespace PromotionMod.Stats.Berserker;

/// <summary>
///     Berserker Class Condition
///     Berserkers gain stats based on # of debuffs on themselves + the damage missing.
///     Tier 1: Missing 25% HP - Gain 10% Damage Reduction - Berserk Defiance
///     Tier 2: Missing 50% HP - Increased Phys Damage and Hit Chance - Berserk Anger
///     Tier 3: Missing 75% HP - Increased Speed. - Berserk Fury
/// </summary>
public class ConBerserker : ClassCondition
{
    // Every Debuff grants 1.
    // Every 5% HP Missing grants 1
    // For every Point, 
    public int GetBerserkPower()
    {
        int debuffs = owner.conditions.Count(x => x.Type == ConditionType.Debuff);
        int hpChunks = (int)(owner.MaxHP * 0.05F);
        int hpBuff = (owner.MaxHP - owner.hp) / hpChunks;
        return hpBuff + debuffs;
    }

    public override void Tick()
    {
        if (owner.hp <= (int)(owner.MaxHP * .75F))
        {
            ConBerserkDefiance? defiance = owner.GetCondition<ConBerserkDefiance>() ?? owner.AddCondition<ConBerserkDefiance>() as ConBerserkDefiance;
            defiance?.Refresh();
        }
        if (owner.hp <= (int)(owner.MaxHP * .5F))
        {
            ConBerserkAnger? anger = owner.GetCondition<ConBerserkAnger>() ?? owner.AddCondition<ConBerserkAnger>() as ConBerserkAnger;
            anger?.Refresh();
        }
        if (owner.hp <= (int)(owner.MaxHP * .25F))
        {
            ConBerserkFury? fury = owner.GetCondition<ConBerserkFury>() ?? owner.AddCondition<ConBerserkFury>() as ConBerserkFury;
            fury?.Refresh();
        }

        int unrelentingPower = GetBerserkPower();
        if (unrelentingPower != 0)
        {
            if (!owner.HasCondition<ConUnrelenting>())
            {
                owner.AddCondition<ConUnrelenting>(unrelentingPower);
            }
            else
            {
                owner.AddCondition<ConUnrelenting>(unrelentingPower);
            }
        }
    }
}
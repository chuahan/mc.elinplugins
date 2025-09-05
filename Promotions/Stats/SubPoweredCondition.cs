using System;
using Newtonsoft.Json;
namespace PromotionMod.Stats;

public class SubPoweredCondition : BaseBuff
{
    /// <summary>
    /// For some Conditions, they use the base Power to apply the effect,
    /// but it's far easier for me to just use PowerOverride to determine the strength, using P2 in the stat sheet.
    /// </summary>
    [JsonProperty(PropertyName = "S")] private int _powerOverride = 1;
    public override int P2 => _powerOverride;
    
    public override int GetPhase()
    {
        return 0;
    }
    
    public static SubPoweredCondition Create(string alias, int power, int powerOverride, Action<Condition> onCreate = null)
    {
        SourceStat.Row row = sources.stats.alias[alias];
        SubPoweredCondition condition = ClassCache.Create<SubPoweredCondition>(row.type.IsEmpty(alias), "Elin");
        condition.power = power;
        condition._powerOverride = powerOverride;
        condition.id = row.id;
        condition._source = row;
        onCreate?.Invoke(condition);
        return condition;
    }
}
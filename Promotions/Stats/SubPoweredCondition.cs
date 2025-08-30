using Newtonsoft.Json;
namespace PromotionMod.Stats;

public class SubPoweredCondition : BaseBuff
{
    /// <summary>
    /// For some Conditions, they use the base Power to apply the effect,
    /// but it's far easier for me to just use PowerOverride to determine the strength, using P2 in the stat sheet.
    /// </summary>
    [JsonProperty(PropertyName = "S")] private int PowerOverride = 1;
    public override int P2 => PowerOverride;
    
    public override int GetPhase()
    {
        return 0;
    }
    
}
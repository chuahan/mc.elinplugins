using Newtonsoft.Json;
namespace PromotionMod.Stats.Maia;

public class ConGateway : Condition
{

    public override bool TimeBased => true;

    [JsonProperty(PropertyName = "D")] public int DestinationUid { get; set; }
}
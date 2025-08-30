using Newtonsoft.Json;
namespace PromotionMod.Stats.Necromancer;

public class ConDeadBeckon : BaseDebuff
{
    [JsonProperty(PropertyName = "N")] public int NecromancerUID;
}
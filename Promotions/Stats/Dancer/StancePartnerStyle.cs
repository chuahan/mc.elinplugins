using Newtonsoft.Json;
namespace PromotionMod.Stats.Dancer;

public class StancePartnerStyle : BaseStance
{
    [JsonProperty(PropertyName = "N")] public int PartnerUID;

    public override void Tick()
    {
        if (_map.FindChara(PartnerUID) == null) Kill();
    }
}
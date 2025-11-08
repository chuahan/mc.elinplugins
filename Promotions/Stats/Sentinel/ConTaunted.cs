using Newtonsoft.Json;
namespace PromotionMod.Stats.Sentinel;

public class ConTaunted : BaseDebuff
{
    [JsonProperty(PropertyName = "T")] public int TaunterUID;

    public override bool TimeBased => true;
    public override ConditionType Type => ConditionType.Debuff;

    public override void Tick()
    {
        Chara taunter = _map.zone.FindChara(TaunterUID);
        if (taunter != null) owner.SetEnemy(taunter);
        base.Tick();
    }
}
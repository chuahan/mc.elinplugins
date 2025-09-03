using System.Collections.Generic;
using Newtonsoft.Json;
using PromotionMod.Common;
namespace PromotionMod.Stats.Sentinel;

public class ConTaunted : BaseDebuff
{
    [JsonProperty(PropertyName = "N")] public int TaunterUID;
    
    public override bool TimeBased => true;
    public override ConditionType Type => ConditionType.Debuff;

    public override void Tick()
    {
        Chara taunter = EClass._map.zone.FindChara(this.TaunterUID);
        owner.SetEnemy(taunter);
        base.Tick();
    }
}
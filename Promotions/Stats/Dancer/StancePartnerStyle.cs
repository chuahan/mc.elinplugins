using System.Collections.Generic;
using Newtonsoft.Json;
namespace PromotionMod.Stats.Dancer;

public class StancePartnerStyle : BaseStance
{
    [JsonProperty(PropertyName = "N")] public int PartnerUID;

    public override void Tick()
    {
        if (_map.FindChara(PartnerUID) == null) Kill();
    }

    public override void OnWriteNote(List<string> list)
    {
        Chara partner = _map.FindChara(PartnerUID);
        if (partner != null) list.Add("hintPartnerStyle".lang(partner.NameSimple));
    }
}
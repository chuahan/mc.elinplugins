using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
namespace PromotionMod.Stats.Dancer;

public class StancePartnerStyle : PromotionStance
{
    [JsonProperty(PropertyName = "N")] public int PartnerUID;

    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }

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
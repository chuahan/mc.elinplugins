using System.Collections.Generic;
using Newtonsoft.Json;
using PromotionMod.Common;
namespace PromotionMod.Stats.Ranger;

public class ConGimmickCoating : BaseBuff
{
    [JsonProperty(PropertyName = "C")]
    public string GimmickType = nameof(Constants.RangerCoatings.HammerCoating);

    // When starting up, apply random Gimmick Type.
    public override void OnStart()
    {
        this.GimmickType = HelperFunctions.GetRandomEnum<Constants.RangerCoatings>();
    }
    
    // Degrades when firing shots. 
    public override void Tick() { }
    
    public override void OnWriteNote(List<string> list)
    {
        string gimmickCoatingHint = "hintRanger_" + this.GimmickType;
        list.Add(gimmickCoatingHint.lang());
    }
}
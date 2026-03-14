using Newtonsoft.Json;
using UnityEngine;
namespace PromotionMod.Stats.Necromancer;

public class ConDeadBeckon : BaseDebuff
{
    [JsonProperty(PropertyName = "N")] public int NecromancerUID;
    
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}
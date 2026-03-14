using Newtonsoft.Json;
using UnityEngine;
namespace PromotionMod.Stats.Sovereign;

public class ConOrderSword : BaseBuff
{
    // Follow-up attack can only occur if this is true. Resets every turn.
    [JsonProperty(PropertyName = "F")] public bool FollowUpAvailable = true;

    public override bool TimeBased => true;
    public override Sprite GetSprite() => SpriteSheet.Get(source.alias);

    public override void Tick()
    {
        FollowUpAvailable = true;
        base.Tick();
    }
}
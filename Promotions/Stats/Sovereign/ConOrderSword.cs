using Newtonsoft.Json;
namespace PromotionMod.Stats.Sovereign;

public class ConOrderSword : BaseBuff
{
    // Follow-up attack can only occur if this is true. Resets every turn.
    [JsonProperty(PropertyName = "F")] public bool FollowUpAvailable = true;

    public override bool TimeBased => true;
    
    public override void Tick()
    {
        this.FollowUpAvailable = true;
        base.Tick();
    }
}
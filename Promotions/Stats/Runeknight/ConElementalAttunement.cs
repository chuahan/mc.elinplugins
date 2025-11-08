using Newtonsoft.Json;
using PromotionMod.Common;
namespace PromotionMod.Stats.Runeknight;

public class ConElementalAttunement : BaseBuff
{

    [JsonProperty(PropertyName = "E")] public int AttunedElement;

    [JsonProperty(PropertyName = "D")] public long StoredDamage;

    public override bool TimeBased => true;

    public override void OnStart()
    {
        StoredDamage = 0;
        AttunedElement = Constants.EleFire;
        base.OnStart();
    }

    public override void Tick()
    {
        if (StoredDamage > 0) StoredDamage = (int)(StoredDamage * 0.95F);
    }
}
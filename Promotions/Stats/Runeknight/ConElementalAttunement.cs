using Newtonsoft.Json;
using PromotionMod.Common;
namespace PromotionMod.Stats.Runeknight;

public class ConElementalAttunement : BaseBuff
{
    [JsonProperty(PropertyName = "D")]
    public int StoredDamage;

    [JsonProperty(PropertyName = "E")] public int AttunedElement;

    public override bool TimeBased => true;

    public override void OnStart()
    {
        this.StoredDamage = 0;
        this.AttunedElement = Constants.EleFire;
        base.OnStart();
    }

    public override void Tick()
    {
        if (StoredDamage > 0) StoredDamage = (int)(StoredDamage * 0.95F);
    }
}
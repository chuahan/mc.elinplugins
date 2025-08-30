using Newtonsoft.Json;
using PromotionMod.Common;
namespace PromotionMod.Stats.Runeknight;

public class ConElementalAttunement : BaseBuff
{
    [JsonProperty(PropertyName = "D")]
    public int StoredDamage = 0;
    [JsonProperty(PropertyName = "E")]
    public int AttunedElement = Constants.EleFire;

    public override bool TimeBased => true;
    
    public override void Tick()
    {
        if (StoredDamage > 0) StoredDamage = (int)(StoredDamage * 0.95F);
    }
}
using Newtonsoft.Json;
namespace PromotionMod.Stats;

public abstract class ClassCondition : Condition
{
    [JsonProperty(PropertyName = "S")] private int Stacks = 1;

    /*
     * Class conditions are non expiring conditions that are containers for data.
     * This is basically to add a ton of information and calculation options for a
     * promotion class to be used in patching to apply effects.
     */
    public override void Tick()
    {
    }
}
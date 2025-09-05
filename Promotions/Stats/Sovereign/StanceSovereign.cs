using Newtonsoft.Json;
namespace PromotionMod.Stats.Sovereign;

public class StanceSovereign : BaseStance
{
    // While in a Sovereign Stance, gain stacks of Sovereign which improves your performance.
    // This caps at 30.
    [JsonProperty(PropertyName = "S")] public int Stacks = 1;

    public int MaxStacks = 30;

    public override void Tick()
    {
        Stacks++;
        if (Stacks > MaxStacks)
        {
            Stacks = MaxStacks;
        }
    }
}
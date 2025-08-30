using System;
using Newtonsoft.Json;
namespace PromotionMod.Stats.Phantom;

public class ConPhantomMark : BaseDebuff
{
    [JsonProperty(PropertyName = "S")] public int Stacks = 0;

    public void AddStacks(int stacks)
    {
        Stacks += stacks;
        Stacks = Math.Min(10, Stacks);
        Mod(3); // Extend Duration by 3.
    }
}
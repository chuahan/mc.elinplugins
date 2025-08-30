using System;
using Newtonsoft.Json;
namespace PromotionMod.Stats;

/// <summary>
/// Class conditions are non expiring conditions that are containers for data.
/// This is basically to add a ton of information and calculation options for a
/// promotion class to be used in patching to apply effects.
/// </summary>
public abstract class ClassCondition : Condition
{
    [JsonProperty(PropertyName = "S")] private int Stacks = 1;

    public virtual int MaxStacks => 30;
    
    public int GetStacks()
    {
        return Math.Max(this.Stacks, MaxStacks);
    }

    public void AddStacks(int stacks)
    {
        this.Stacks += stacks;
        this.Stacks = Math.Min(this.Stacks, MaxStacks);
    }

    public override void Tick()
    {
    }
}
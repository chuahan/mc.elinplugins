using System;
using Newtonsoft.Json;
using UnityEngine;
namespace PromotionMod.Stats;

/// <summary>
///     Class conditions are non expiring conditions that are containers for data.
///     This is basically to add a ton of information and calculation options for a
///     promotion class to be used in patching to apply effects.
/// </summary>
public abstract class ClassCondition : Condition
{
    [JsonProperty(PropertyName = "S")] private int Stacks = 1;

    public virtual int MaxStacks => 30;

    public override string TextDuration => "";
    
    [JsonProperty(PropertyName = "R")] public int DecayDelay;
    
    public virtual int DecayDelayMax => -1;
    
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
    
    public int GetStacks()
    {
        return Math.Max(Stacks, MaxStacks);
    }

    public void AddStacks(int stacks)
    {
        Stacks += stacks;
        Stacks = Math.Min(Stacks, MaxStacks);
        if (DecayDelayMax != -1) DecayDelay = 0;
    }

    // If the stacks need to decay.
    protected void TickStacks()
    {
        Stacks--;
        Stacks = Math.Max(0, Stacks);
    }
    
    public override void Tick()
    {
        if (DecayDelayMax != -1)
        {
            if (DecayDelay == DecayDelayMax)
            {
                TickStacks();
            }
            else
            {
                DecayDelay++;
            }
        }
    }
}
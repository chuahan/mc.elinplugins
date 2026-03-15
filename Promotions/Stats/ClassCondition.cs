using System;
using Cwl.Helper.Extensions;
using Newtonsoft.Json;
using PromotionMod.Common;
using UnityEngine;
namespace PromotionMod.Stats;

/// <summary>
///     Class conditions are non expiring conditions that are containers for data.
///     This is basically to add a ton of information and calculation options for a
///     promotion class to be used in patching to apply effects.
/// </summary>
public abstract class ClassCondition : Timebuff
{

    [JsonProperty(PropertyName = "R")] public int DecayDelay;
    [JsonProperty(PropertyName = "S")] private int Stacks = 0;

    public virtual int MaxStacks => 30;

    public override string TextDuration => "";

    public virtual int DecayDelayMax => -1;

    public virtual int PromotionClass => -1;

    public virtual bool CanExpire => false;
    
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }

    public int GetStacks()
    {
        return Math.Min(Stacks, MaxStacks);
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
        // If at any point, the character with this condition does not match this... AKA, you had it before and then class changed, kill the condition.
        if (!owner.MatchesPromotion(this.PromotionClass)) this.Kill();

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
        
        if (CanExpire) base.Tick();
    }
}
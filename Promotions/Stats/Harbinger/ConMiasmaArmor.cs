using System;
using Newtonsoft.Json;
namespace PromotionMod.Stats.Harbinger;

public class ConMiasmaArmor : Timebuff
{
    [JsonProperty(PropertyName = "S")] private int Stacks = 1;

    public override int EvaluatePower(int p)
    {
        if (p > Stacks) Stacks = p;
        return GetStacks();
    }

    public int GetStacks()
    {
        return Math.Min(Stacks, 19);
    }
}
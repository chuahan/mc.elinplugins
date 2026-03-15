using System;
using Newtonsoft.Json;
using UnityEngine;
namespace PromotionMod.Stats.Harbinger;

public class ConMiasmaArmor : Timebuff
{
    [JsonProperty(PropertyName = "S")] private int _stacks = 1;

    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }

    public override int EvaluatePower(int p)
    {
        if (p > _stacks) _stacks = p;
        return GetStacks();
    }

    public int GetStacks()
    {
        return Math.Min(_stacks, 19);
    }
}
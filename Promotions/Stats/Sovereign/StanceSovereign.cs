using Newtonsoft.Json;
using UnityEngine;
namespace PromotionMod.Stats.Sovereign;

public class StanceSovereign : BaseStance
{
    public int MaxStacks = 10;

    // While in a Sovereign Stance, gain stacks of Sovereign which improves your performance. Caps at 10.
    [JsonProperty(PropertyName = "S")] public int Stacks = 1;

    public override bool TimeBased => true;

    public override string TextDuration => Stacks.ToString();

    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }

    public override void Tick()
    {
        Stacks++;
        if (Stacks > MaxStacks)
        {
            Stacks = MaxStacks;
        }
    }
}
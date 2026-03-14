using Newtonsoft.Json;
using UnityEngine;
namespace PromotionMod.Stats.Sovereign;

public class StanceSovereign : BaseStance
{
    public int MaxStacks = 10;
    
    public override bool TimeBased => true;

    // While in a Sovereign Stance, gain stacks of Sovereign which improves your performance. Caps at 10.
    [JsonProperty(PropertyName = "S")] public int Stacks = 1;
    
    public override string TextDuration => Stacks.ToString();
    
    public override Sprite GetSprite() => SpriteSheet.Get(source.alias);
    
    public override void Tick()
    {
        Stacks++;
        if (Stacks > MaxStacks)
        {
            Stacks = MaxStacks;
        }
    }
}
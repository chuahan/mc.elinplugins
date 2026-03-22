using UnityEngine;
namespace PromotionMod.Stats.WarCleric;

public class ConShiningBlade : BaseBuff
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
    
    // Stack based Condition.
    public override void Tick()
    {
    }
}
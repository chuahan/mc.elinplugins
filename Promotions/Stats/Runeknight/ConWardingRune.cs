using UnityEngine;
namespace PromotionMod.Stats.Runeknight;

public class ConWardingRune : BaseBuff
{
    public override bool TimeBased => true;

    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
    
    // Stack based Condition instead.
    public override void Tick()
    {
    }

    public override bool CanStack(Condition c)
    {
        return true;
    }
}
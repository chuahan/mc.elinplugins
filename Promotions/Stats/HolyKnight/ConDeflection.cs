using UnityEngine;
namespace PromotionMod.Stats.HolyKnight;

public class ConDeflection : BaseBuff
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
    
    public override bool TimeBased => true;
}
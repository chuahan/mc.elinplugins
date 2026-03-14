using UnityEngine;
namespace PromotionMod.Stats.Knightcaller;

public class ConKnightlySpirit : BaseBuff
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
    
    public override bool CanManualRemove => true;
}
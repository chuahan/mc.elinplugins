using UnityEngine;
namespace PromotionMod.Stats.Knightcaller;

public class ConSpiritRally : BaseBuff
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}
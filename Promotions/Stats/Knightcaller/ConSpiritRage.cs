using UnityEngine;
namespace PromotionMod.Stats.Knightcaller;

public class ConSpiritRage : BaseBuff
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}
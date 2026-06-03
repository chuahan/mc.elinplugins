using UnityEngine;
namespace PromotionMod.Stats;

public class ConSpiritRage : BaseBuff
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}
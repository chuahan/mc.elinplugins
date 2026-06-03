using UnityEngine;
namespace PromotionMod.Stats;

public class ConUnrelenting : BaseBuff
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}
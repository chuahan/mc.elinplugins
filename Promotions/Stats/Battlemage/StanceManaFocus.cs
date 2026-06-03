using UnityEngine;
namespace PromotionMod.Stats;

public class StanceManaFocus : PromotionStance
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}
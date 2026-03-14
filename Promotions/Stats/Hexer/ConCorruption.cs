using UnityEngine;
namespace PromotionMod.Stats.Hexer;

// Stop all sources of healing
public class ConCorruption : Timebuff
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}
using UnityEngine;
namespace PromotionMod.Stats.Hexer;

// Reduces strength/will
public class ConMalaise : Timebuff
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}
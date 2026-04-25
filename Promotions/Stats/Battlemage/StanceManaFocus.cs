using UnityEngine;
namespace PromotionMod.Stats.Battlemage;

public class StanceManaFocus : PromotionStance
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}
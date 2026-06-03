using UnityEngine;
namespace PromotionMod.Stats;

public class StanceRage : PromotionStance
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}
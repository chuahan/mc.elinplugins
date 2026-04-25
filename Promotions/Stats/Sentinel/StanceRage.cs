using UnityEngine;
namespace PromotionMod.Stats.Sentinel;

public class StanceRage : PromotionStance
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}
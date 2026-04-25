using UnityEngine;
namespace PromotionMod.Stats.Sentinel;

public class StanceRestraint : PromotionStance
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}
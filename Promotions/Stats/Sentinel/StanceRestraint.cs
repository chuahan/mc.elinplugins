using UnityEngine;
namespace PromotionMod.Stats;

public class StanceRestraint : PromotionStance
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}
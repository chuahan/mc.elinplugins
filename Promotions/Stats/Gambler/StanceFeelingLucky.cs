using UnityEngine;
namespace PromotionMod.Stats;

public class StanceFeelingLucky : PromotionStance
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}
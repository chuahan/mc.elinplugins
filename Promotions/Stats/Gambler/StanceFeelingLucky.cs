using UnityEngine;
namespace PromotionMod.Stats.Gambler;

public class StanceFeelingLucky : PromotionStance
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}
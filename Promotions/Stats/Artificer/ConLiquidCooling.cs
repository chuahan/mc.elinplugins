using UnityEngine;
namespace PromotionMod.Stats;

public class ConLiquidCooling : BaseBuff
{
    public override string TextDuration => "";

    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}
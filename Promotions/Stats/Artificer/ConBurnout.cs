using UnityEngine;
namespace PromotionMod.Stats;

public class ConBurnout : BaseBuff
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}
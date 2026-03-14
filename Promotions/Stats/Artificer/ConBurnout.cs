using UnityEngine;
namespace PromotionMod.Stats.Artificer;

public class ConBurnout : BaseBuff
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}
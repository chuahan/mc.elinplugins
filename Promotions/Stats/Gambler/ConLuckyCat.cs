using UnityEngine;
namespace PromotionMod.Stats;

public class ConLuckyCat : BaseBuff
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}
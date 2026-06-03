using UnityEngine;
namespace PromotionMod.Stats;

public class ConMarked : BaseDebuff
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}
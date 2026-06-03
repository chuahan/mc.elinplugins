using UnityEngine;
namespace PromotionMod.Stats;

public class ConVigilance : BaseBuff
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}
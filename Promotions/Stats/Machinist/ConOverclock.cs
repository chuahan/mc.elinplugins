using UnityEngine;
namespace PromotionMod.Stats.Machinist;

public class ConOverclock : BaseBuff
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}
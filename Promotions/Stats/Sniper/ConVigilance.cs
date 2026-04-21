using UnityEngine;
namespace PromotionMod.Stats.Sniper;

public class ConVigilance : BaseBuff
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}
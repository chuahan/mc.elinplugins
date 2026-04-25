using UnityEngine;
namespace PromotionMod.Stats.Gambler;

public class ConLuckyCat : BaseBuff
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}
using UnityEngine;
namespace PromotionMod.Stats.Knightcaller;

public class ConKnightlySpirit : BaseBuff
{

    public override bool CanManualRemove => true;
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}
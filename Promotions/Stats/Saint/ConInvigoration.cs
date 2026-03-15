using UnityEngine;
namespace PromotionMod.Stats.Saint;

public class ConInvigoration : BaseBuff
{
    public override bool TimeBased => true;
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}
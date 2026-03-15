using UnityEngine;
namespace PromotionMod.Stats.Saint;

public class ConBlessing : BaseBuff
{

    public override bool TimeBased => true;
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}
using UnityEngine;
namespace PromotionMod.Stats;

public class ConUnderFire : BaseDebuff
{

    public override bool TimeBased => true;
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}
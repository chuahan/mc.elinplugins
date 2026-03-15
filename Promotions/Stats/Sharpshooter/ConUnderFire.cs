using UnityEngine;
namespace PromotionMod.Stats.Sharpshooter;

public class ConUnderFire : BaseDebuff
{

    public override bool TimeBased => true;
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}
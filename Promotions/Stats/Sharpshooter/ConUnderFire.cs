using UnityEngine;
namespace PromotionMod.Stats.Sharpshooter;

public class ConUnderFire : BaseDebuff
{
    public override Sprite GetSprite() => SpriteSheet.Get(source.alias);
    
    public override bool TimeBased => true;
}
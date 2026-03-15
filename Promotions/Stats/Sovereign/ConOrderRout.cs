using UnityEngine;
namespace PromotionMod.Stats.Sovereign;

public class ConOrderRout : BaseBuff
{
    public override bool TimeBased => true;
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}
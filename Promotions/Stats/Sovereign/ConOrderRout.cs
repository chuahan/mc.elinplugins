using UnityEngine;
namespace PromotionMod.Stats.Sovereign;

public class ConOrderRout : BaseBuff
{
    public override Sprite GetSprite() => SpriteSheet.Get(source.alias);
    public override bool TimeBased => true;
}
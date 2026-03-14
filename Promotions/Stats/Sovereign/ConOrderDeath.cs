using UnityEngine;
namespace PromotionMod.Stats.Sovereign;

public class ConOrderDeath : BaseBuff
{
    public override Sprite GetSprite() => SpriteSheet.Get(source.alias);
    public override bool TimeBased => true;
}
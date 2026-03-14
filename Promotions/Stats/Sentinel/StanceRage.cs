using UnityEngine;
namespace PromotionMod.Stats.Sentinel;

public class StanceRage : BaseStance
{
    public override Sprite GetSprite() => SpriteSheet.Get(source.alias);
}
using UnityEngine;
namespace PromotionMod.Stats.Sentinel;

public class StanceRage : BaseStance
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}
using UnityEngine;
namespace PromotionMod.Stats;

public class ConHeavenlyEmbrace : BaseBuff
{
    public override void Tick()
    {
        // Do not tick.
    }

    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}
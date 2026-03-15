using UnityEngine;
namespace PromotionMod.Stats.WarCleric;

public class ConShiningBlade : BaseBuff
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
    public override void Tick()
    {
    }
}
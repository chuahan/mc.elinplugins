using UnityEngine;
namespace PromotionMod.Stats.Sentinel;

public class StanceRestraint : BaseStance
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}
using UnityEngine;
namespace PromotionMod.Stats.Sentinel;

public class StanceRestraint : BaseStance
{
    public override Sprite GetSprite() => SpriteSheet.Get(source.alias);
}
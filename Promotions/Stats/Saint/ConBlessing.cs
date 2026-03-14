using UnityEngine;
namespace PromotionMod.Stats.Saint;

public class ConBlessing : BaseBuff
{
    public override Sprite GetSprite() => SpriteSheet.Get(source.alias);

    public override bool TimeBased => true;
}
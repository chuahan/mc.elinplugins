using UnityEngine;
namespace PromotionMod.Stats.Battlemage;

public class StanceManaFocus : BaseStance
{

    public override string TextDuration => "";
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}
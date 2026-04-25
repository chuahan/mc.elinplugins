using UnityEngine;
namespace PromotionMod.Stats;

public class ConArmorBoost : SubPoweredCondition
{
    public override ConditionType Type => ConditionType.Buff;
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}
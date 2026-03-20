using UnityEngine;
namespace PromotionMod.Stats;

public class ConCritBoost : SubPoweredCondition
{
    public override ConditionType Type => ConditionType.Buff;
    public override bool TimeBased => true;
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
}
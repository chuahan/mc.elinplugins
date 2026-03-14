using UnityEngine;
namespace PromotionMod.Stats.Knightcaller;

public class ConSummoningSickness : BadCondition
{
    public override Sprite GetSprite()
    {
        return SpriteSheet.Get(source.alias);
    }
    
    public override ConditionType Type => ConditionType.Bad;
    public override bool TimeBased => true;
}
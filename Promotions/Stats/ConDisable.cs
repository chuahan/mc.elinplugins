using UnityEngine;
namespace PromotionMod.Stats;

public class ConDisable : BadCondition
{
    public override Sprite GetSprite() => SpriteSheet.Get(source.alias);
    public override ConditionType Type => ConditionType.Debuff;
    public override bool TimeBased => true;
}